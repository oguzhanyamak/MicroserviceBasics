using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumer
{
    public class PaymentFailedEventConsumer :IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDbContext _context;

        public PaymentFailedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            Console.WriteLine("O -> Payment Failed");
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.Status = Models.Enums.OrderStatus.Failed;
            await _context.SaveChangesAsync();

        }
    }
}
