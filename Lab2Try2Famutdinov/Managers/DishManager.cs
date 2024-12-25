using Lab2Try2Famutdinov.Controllers;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<Dish>> GetDishesAsync()
        {
            return await _context.Dish.ToListAsync();
        }

        // Получение блюда по ID
        public async Task<Dish> GetDishAsync(int id)
        {
            return await _context.Dish
                                 .Include(d => d.Orders) // Подключаем Orders для загрузки блюд в заказах
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }


        // Создание нового блюда
        public async Task<Dish> AddDishAsync(Dish dish)
        {
            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();
            return dish;
        }

        // Обновление блюда
        public async Task<Dish> UpdateDishAsync(int id, Dish updatedDish)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                throw new Exception("Dish not found");
            }

            dish.Name = updatedDish.Name;
            dish.Price = updatedDish.Price;
            dish.IsAvailable = updatedDish.IsAvailable;

            await _context.SaveChangesAsync();

            return dish;
        }

        public async Task<List<Dish>> GetAvailableDishesSortedByPriceAsync()
        {
            return await _context.Dish
                                 .Where(d => d.IsAvailable)  // Фильтрация по доступности
                                 .OrderBy(d => d.Price)      // Сортировка по цене
                                 .ToListAsync();
        }

    }
}

