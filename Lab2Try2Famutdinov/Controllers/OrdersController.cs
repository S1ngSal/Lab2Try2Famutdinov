using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;

namespace Lab2Try2Famutdinov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public OrdersController(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Order.Include(o => o.Dishes).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order
                                      .Include(o => o.Dishes)
                                      .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order, [FromHeader] string role)
        {
            // Проверка роли
            if (string.IsNullOrEmpty(role) || !role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("Только клиенты могут создавать заказы.");
            }

            // Получение всех блюд из базы данных
            var allDishes = await _context.Dish.ToListAsync();

            // Проверка на существование и доступность блюд
            var invalidDishes = new List<Dish>();
            float totalPrice = 0;

            var validDishes = new List<Dish>();
            foreach (var dish in order.Dishes)
            {
                var existingDish = allDishes.FirstOrDefault(d => d.Id == dish.Id);

                if (existingDish == null || !existingDish.IsAvailable)
                {
                    invalidDishes.Add(dish);
                }
                else
                {
                    totalPrice += existingDish.Price;
                    validDishes.Add(existingDish); // Добавляем только валидные блюда
                }
            }

            if (invalidDishes.Any())
            {
                return BadRequest(new
                {
                    Message = "Некоторые из указанных блюд не существуют или недоступны.",
                    InvalidDishes = invalidDishes
                });
            }

            // Создаем заказ с привязкой к существующим блюдам
            var newOrder = new Order
            {
                UserId = order.UserId,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                Dishes = validDishes // Привязываем только валидные блюда
            };

            // Добавляем заказ в базу данных
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }


        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id, [FromHeader] string role)
        {
            // Проверка роли
            if (string.IsNullOrEmpty(role) || !role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("Только администратор может удалять заказы.");
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
