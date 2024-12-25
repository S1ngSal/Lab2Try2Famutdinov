using Lab2Try2Famutdinov.Controllers;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderManager
{
    private readonly Lab2Try2FamutdinovContext _context;

    public OrderManager(Lab2Try2FamutdinovContext context)
    {
        _context = context;
    }

    // Получение всех заказов
    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Order
                             .Include(o => o.OrderItems)  // Загружаем блюда для каждого заказа
                             .ToListAsync();
    }

    // Получение заказа по ID
    public async Task<Order> GetOrderAsync(int id)
    {
        var order = await _context.Order
                                  .Include(o => o.OrderItems)
                                  .FirstOrDefaultAsync(o => o.Id == id);

        if (order != null)
        {
            foreach (var orderItem in order.OrderItems)
            {
                var dish = await _context.Dish
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(d => d.Id == orderItem.DishId);

                if (dish != null)
                {
                    orderItem.DishName = dish.Name;
                    orderItem.DishPrice = dish.Price;
                }
            }
        }

        return order;
    }




    // Создание нового заказа
    // Создание нового заказа с возможностью добавления одинаковых блюд
    public async Task<Order> CreateOrderAsync(Order order)
    {
        if (order.OrderItems == null || order.OrderItems.Count == 0)
        {
            throw new Exception("Order must contain at least one dish.");
        }

        float totalPrice = 0;  // Переменная для хранения общей стоимости заказа

        foreach (var orderItem in order.OrderItems)
        {
            var existingDish = await _context.Dish
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(d => d.Id == orderItem.DishId);

            if (existingDish == null)
            {
                throw new Exception($"Dish with ID {orderItem.DishId} not found.");
            }

            // Проверяем, существует ли уже такой элемент в заказе
            var existingOrderItem = order.OrderItems
                .FirstOrDefault(o => o.DishId == orderItem.DishId);

            if (existingOrderItem == null)
            {
                // Если такой позиции нет, добавляем ее в OrderItems
                order.OrderItems.Add(new OrderItem
                {
                    DishId = existingDish.Id,
                    Quantity = orderItem.Quantity,
                    DishName = existingDish.Name,  // Присваиваем название блюда
                    DishPrice = existingDish.Price
                });

                // Добавляем стоимость этого блюда в общую цену
                totalPrice += orderItem.DishPrice * orderItem.Quantity;
            }
            else
            {
                // Если такая позиция уже есть, просто увеличиваем количество
                totalPrice += existingOrderItem.DishPrice * orderItem.Quantity;
            }
        }

        // Присваиваем рассчитанную общую стоимость заказу
        order.TotalPrice = totalPrice;

        // Добавляем заказ в контекст
        _context.Order.Add(order);

        // Сохраняем изменения в базе данных
        await _context.SaveChangesAsync();

        return order;
    }


}


