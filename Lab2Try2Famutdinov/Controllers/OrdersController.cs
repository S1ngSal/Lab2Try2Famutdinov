using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2Try2Famutdinov.Data;
using Lab2Try2Famutdinov.Models;
using Microsoft.AspNetCore.Authorization;
using Lab2Try2Famutdinov.Managers;

namespace Lab2Try2Famutdinov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderManager _orderManager;

        public OrdersController(OrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _orderManager.GetOrdersAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            return await _orderManager.GetOrderAsync(id);
        }

        // POST: api/Orders/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            return await _orderManager.CreateOrderAsync(order);
        }

    }
}
