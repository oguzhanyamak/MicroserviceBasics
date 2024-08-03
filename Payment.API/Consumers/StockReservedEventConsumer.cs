using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _endpoint;

        public StockReservedEventConsumer(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            bool randomBool = new Random().Next(2) == 0;
            Console.WriteLine($"Payment Status : {randomBool}");
            if (randomBool)
            {
                Console.WriteLine("P -> Payment Success");
                await _endpoint.Publish(new PaymentCompletedEvent(){OrderId = context.Message.OrderId});
            }
            else
            {
                Console.WriteLine("P -> Payment Failed");
                await _endpoint.Publish(new PaymentFailedEvent() { OrderId = context.Message.OrderId, Message = "Order Failed" });
            }
        }
    }
}
