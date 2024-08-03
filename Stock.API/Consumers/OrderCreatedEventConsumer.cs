using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Models.Entities;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {

        IMongoCollection<Stock.API.Models.Entities.Stock> _stockCollection;
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IPublishEndpoint _publishEndpoint;
        public OrderCreatedEventConsumer(MongoDbService dbService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _stockCollection = dbService.GetCollection<Stock.API.Models.Entities.Stock>();
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (OrderItemMessage item in context.Message.OrderItems)
            {
                stockResult.Add((await _stockCollection.FindAsync(s => s.ProductId == item.ProductId && s.Count >= item.Count)).Any());
            }
            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage item in context.Message.OrderItems)
                {
                    var prod = await (await _stockCollection.FindAsync(s => s.ProductId == item.ProductId)).FirstOrDefaultAsync();
                    prod.Count -= item.Count;
                    await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, prod);
                }

                StockReservedEvent SRevent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                };
                Console.WriteLine("S -> Stock Reserve");
                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                await sendEndpoint.Send(SRevent);
            }
            else
            {
                Console.WriteLine("S -> Stock Not Reserve");
                await _publishEndpoint.Publish(new StockNotReservedEvent() { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message = "Yeterli Stock Bulunamadı" });
            }
        }
    }
}
