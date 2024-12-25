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
    public class UserManager
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public UserManager(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // Получение всех пользователей
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            return await _context.User.ToListAsync();
        }

        // Получение пользователя по id
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return new ActionResult<User>(new NotFoundResult());
            }
            return user;
        }

        // Добавление пользователя
        public async Task<ActionResult<User>> AddUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Role) || (!user.IsAdmin() && !user.IsCustomer()))
            {
                return new BadRequestObjectResult("Роль должна быть 'Admin' или 'Customer'.");
            }

            if (user.IsCustomer() && !user.IsCustomerDataValid())
            {
                return new BadRequestObjectResult("Для роли 'Customer' требуется указать Name, Phone и Address.");
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(
            nameof(UsersController.GetUser),  // Имя метода контроллера, который будет обрабатывать запрос
            "Users",                          // Имя контроллера
            new { id = user.Id },             // Параметры маршрута (ID нового пользователя)
            user                              // Сам объект пользователя
            );
        }

        // Обновление пользователя
        public async Task<IActionResult> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return new BadRequestResult();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.User.Any(e => e.Id == id))
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

        // Удаление пользователя
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<List<User>> GetUsersWithOrdersAsync()
        {
            return await _context.User
                                 .Where(u => _context.Order.Any(o => o.UserId == u.Id)) // Проверяем наличие заказов у пользователя
                                 .ToListAsync();
        }

    }
}
