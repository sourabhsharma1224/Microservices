using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(OrderDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            // Validate User
            var id = order.UserId;
            var userClient = _httpClientFactory.CreateClient("UserService");
            var userResponse = await userClient.GetAsync($"{id}");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest("Invalid UserId");

            // Validate Book
            var bookid = order.BookId;
            var bookClient = _httpClientFactory.CreateClient("BookService");
            var bookResponse = await bookClient.GetAsync($"{bookid}");
            if (!bookResponse.IsSuccessStatusCode)
                return BadRequest("Invalid BookId");

            // Save Order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
        }
    }
}
