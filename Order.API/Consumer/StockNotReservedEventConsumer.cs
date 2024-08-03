using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly OrderAPIDbContext _context;

        public StockNotReservedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Console.WriteLine("O -> StockNotReserved");
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.Status = Models.Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();

        }
    }
}
