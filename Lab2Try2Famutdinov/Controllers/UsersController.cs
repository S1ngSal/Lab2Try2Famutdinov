using Lab2Try2Famutdinov.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab2Try2Famutdinov.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2Try2Famutdinov.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager _userManager;

        public UsersController(UserManager userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userManager.GetUsersAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            return await _userManager.GetUserAsync(id);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            return await _userManager.UpdateUserAsync(id, user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            return await _userManager.AddUserAsync(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userManager.DeleteUserAsync(id);
        }

        // GET: api/Users/WithOrders
        [HttpGet("WithOrders")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersWithOrders()
        {
            var usersWithOrders = await _userManager.GetUsersWithOrdersAsync();
            return Ok(usersWithOrders);  // Возвращаем список пользователей с кодом 200.
        }

    }
}
