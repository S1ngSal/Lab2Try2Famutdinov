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
    public class DishesController : ControllerBase
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public DishesController(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            return await _context.Dish.ToListAsync();
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _context.Dish.FindAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // POST: api/Dishes (только для администраторов)
        [HttpPost("AddDish")]
        public async Task<ActionResult<Dish>> AddDish([FromBody] Dish dish, [FromHeader] string role)
        {
            if (string.IsNullOrEmpty(role) || !role.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("Только администратор может добавлять блюда.");
            }

            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        // PUT: api/Dishes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // DELETE: api/Dishes/5 (только для администраторов)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id, [FromHeader] string role)
        {
            if (string.IsNullOrEmpty(role) || !role.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("Только администратор может удалять блюда.");
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Dishes/Available — получение только доступных блюд
        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAvailableDishes()
        {
            var availableDishes = await _context.Dish
                .Where(d => d.IsAvailable)
                .ToListAsync();

            return Ok(availableDishes);
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}

