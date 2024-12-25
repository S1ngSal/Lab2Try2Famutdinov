using Microsoft.AspNetCore.Http;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2Try2Famutdinov.Data;
using Microsoft.AspNetCore.Authorization;
using Lab2Try2Famutdinov.Managers;

namespace Lab2Try2Famutdinov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DishManager _dishManager;

        public DishesController(DishManager dishManager)
        {
            _dishManager = dishManager;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            var dishes = await _dishManager.GetDishesAsync();
            return Ok(dishes);  // Оборачиваем результат в Ok(), который возвращает статус 200 и данные.
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _dishManager.GetDishAsync(id);
            if (dish == null)
            {
                return NotFound();  // Если блюдо не найдено, возвращаем статус 404.
            }
            return Ok(dish);  // Если блюдо найдено, возвращаем его с статусом 200.
        }

        // POST: api/Dishes/AddDish
        [Authorize(Roles = "admin")]
        [HttpPost("AddDish")]
        public async Task<ActionResult<Dish>> AddDish([FromBody] Dish dish)
        {
            var createdDish = await _dishManager.AddDishAsync(dish);
            return CreatedAtAction(nameof(GetDish), new { id = createdDish.Id }, createdDish);  // Возвращаем статус 201 и созданное блюдо.
        }

        // PUT: api/Dishes/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, [FromBody] Dish dish)
        {
            var updatedDish = await _dishManager.UpdateDishAsync(id, dish);
            if (updatedDish == null)
            {
                return NotFound();  // Если блюдо не найдено, возвращаем статус 404.
            }
            return NoContent();  // Возвращаем статус 204, если обновление прошло успешно.
        }

        // GET: api/Dishes/AvailableSortedByPrice
        [HttpGet("AvailableSortedByPrice")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAvailableDishesSortedByPrice()
        {
            var dishes = await _dishManager.GetAvailableDishesSortedByPriceAsync();
            return Ok(dishes);  // Возвращаем результат в формате 200 OK.
        }

    }
}
