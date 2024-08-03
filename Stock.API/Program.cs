using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Models.Entities;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMq"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue,e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));

    });
});

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    // Resolve the MongoDbService from the service provider
    var service = scope.ServiceProvider.GetRequiredService<MongoDbService>();

    // Get the collection
    var collection = service.GetCollection<Stock.API.Models.Entities.Stock>();

    // Ensure the collection is not null
    //if (collection == null)
    //{
    //    throw new InvalidOperationException("The collection is not available.");
    //}

    // Check if the collection is empty
    var filter = Builders<Stock.API.Models.Entities.Stock>.Filter.Empty;
    var count = await collection.CountDocumentsAsync(filter);

    if (count == 0)
    {
        // Insert documents into the collection
        var documents = new[]
        {
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 2000 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 2000 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 3000 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 4000 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 5000 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 500 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 1500 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 2200 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 1100 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 2300 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 300 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 450 },
                new Stock.API.Models.Entities.Stock { ProductId = Guid.NewGuid().ToString(), Count = 100 }
            };

        await collection.InsertManyAsync(documents);
    }
}




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
