using Lab2Try2Famutdinov.Controllers;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Try2Famutdinov.Managers
{
    public class DishManager
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public DishManager(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // Получение всех блюд
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishesAsync()
        {
            var dishes = await _context.Dish.ToListAsync();
            return dishes;
        }

        // Получение конкретного блюда
        public async Task<ActionResult<Dish>> GetDishAsync(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return new ActionResult<Dish>(new NotFoundResult());
            }
            return dish;
        }

        // Добавление блюда
        public async Task<ActionResult<Dish>> AddDishAsync(Dish dish)
        {
            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();

            // Передаем все четыре аргумента в CreatedAtActionResult
            return new CreatedAtActionResult(
                nameof(DishesController.GetDish),  // Название метода в контроллере
                "Dishes",                          // Имя контроллера
                new { id = dish.Id },              // Параметры маршрута (ID нового блюда)
                dish                               // Возвращаем сам объект блюда
            );
        }


        // Обновление блюда
        public async Task<IActionResult> UpdateDishAsync(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return new BadRequestResult();
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
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        // Удаление блюда
        public async Task<IActionResult> DeleteDishAsync(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return new NotFoundResult();
            }

            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}
