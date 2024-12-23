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

            // Используем правильный конструктор CreatedAtActionResult с четырьмя аргументами
            return new CreatedAtActionResult(
                nameof(UsersController.GetUser),  // Название метода, который будет вызван (например, GetUser)
                "Users",                          // Имя контроллера, в котором находится метод
                new { id = user.Id },             // Параметры маршрута (ID нового пользователя)
                user                              // Сам объект пользователя
            );
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

        // Фильтрация клиентов по телефону
        public IActionResult GetCustomersByPhone(string phone)
        {
            var customers = _context.User
                .AsEnumerable()
                .Where(u => u.IsCustomer() && u.Phone.Contains(phone))
                .ToList();

            return new OkObjectResult(customers);
        }

        // Фильтрация администраторов по email
        public IActionResult GetAdminsByEmail(string emailPart)
        {
            var admins = _context.User
                .AsEnumerable()
                .Where(u => u.IsAdmin() && u.Email.Contains(emailPart))
                .ToList();

            return new OkObjectResult(admins);
        }
    }
}
