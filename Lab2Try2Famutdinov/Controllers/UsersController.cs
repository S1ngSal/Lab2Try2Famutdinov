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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Lab2Try2FamutdinovContext _context;

        public UsersController(Lab2Try2FamutdinovContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Логика создания пользователя с проверками

            if (string.IsNullOrEmpty(user.Role) || (!user.IsAdmin() && !user.IsCustomer()))
            {
                return BadRequest("Роль должна быть 'Admin' или 'Customer'.");
            }

            if (user.IsCustomer() && !user.IsCustomerDataValid())
            {
                return BadRequest("Для роли 'Customer' требуется указать Name, Phone и Address.");
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Функции фильтрации клиентов по телефону
        [HttpGet("customers")]
        public IActionResult GetCustomersByPhone([FromQuery] string phone)
        {
            var customers = _context.User
                .AsEnumerable()
                .Where(u => u.IsCustomer() && u.Phone.Contains(phone))
                .ToList();

            return Ok(customers);
        }

        // Функции фильтрации администраторов по email
        [HttpGet("admins")]
        public IActionResult GetAdminsByEmail([FromQuery] string emailPart)
        {
            var admins = _context.User
                .AsEnumerable()
                .Where(u => u.IsAdmin() && u.Email.Contains(emailPart))
                .ToList();

            return Ok(admins);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
