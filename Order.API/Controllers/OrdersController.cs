using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.VM;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM vm)
        {
            Console.WriteLine("--------------------> Order Call");
            Models.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = vm.BuyerId,
                CreateDate = DateTime.UtcNow,
                Status = Models.Enums.OrderStatus.Suspend
            };
            order.OrderItems = vm.OrderItems.Select(oi => new Models.Entities.OrderItem { Count = oi.Count, Price = oi.Price, ProductId = oi.ProductId.ToString() }).ToList();
            order.TotalPrice = vm.OrderItems.Sum(oi => (oi.Price * oi.Count));

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            OrderCreatedEvent createdEvent = new()
            {
                BuyerId = order.BuyerId,
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage { Count = oi.Count, ProductId = oi.ProductId }).ToList(),
                TotalPrice = order.TotalPrice,
            };

            await _publishEndpoint.Publish(createdEvent);
            return Ok();
        }
    }
}
