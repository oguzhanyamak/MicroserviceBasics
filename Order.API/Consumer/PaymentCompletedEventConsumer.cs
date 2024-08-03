using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _context;

        public PaymentCompletedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Console.WriteLine("O -> Payment Completed");
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.Status = Models.Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();

        }
    }
}
