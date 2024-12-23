using Lab2Try2Famutdinov.Controllers;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Try2Famutdinov.Managers
{
    public class OrderManager
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public OrderManager(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // Создание заказа
        public async Task<ActionResult<Order>> CreateOrderAsync(Order order)
        {
            var allDishes = await _context.Dish.ToListAsync();

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
                    validDishes.Add(existingDish);
                }
            }

            if (invalidDishes.Any())
            {
                return new ActionResult<Order>(new BadRequestObjectResult(new
                {
                    Message = "Некоторые из указанных блюд не существуют или недоступны.",
                    InvalidDishes = invalidDishes
                }));
            }

            var newOrder = new Order
            {
                UserId = order.UserId,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                Dishes = validDishes
            };

            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();

            // Исправление: передаем параметр 'value' как newOrder
            return new ActionResult<Order>(new CreatedAtActionResult(
                nameof(OrdersController.GetOrder),  // Название метода действия
                "Orders",                           // Название контроллера
                new { id = newOrder.Id },           // Параметры маршрута (передаем ID нового заказа)
                newOrder                            // Значение (новый созданный заказ)
            ));
        }


        // Получение заказа
        public async Task<ActionResult<Order>> GetOrderAsync(int id)
        {
            var order = await _context.Order.Include(o => o.Dishes).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return new ActionResult<Order>(new NotFoundResult());
            }

            return new ActionResult<Order>(order);
        }
    }
}
