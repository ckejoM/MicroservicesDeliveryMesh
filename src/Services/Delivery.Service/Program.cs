using Delivery.Service.Data;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<DeliveryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Host.UseWolverine(opts =>
{
    // 1. Setup the connection
    var rabbit = opts.UseRabbitMq(new Uri("amqp://guest:guest@localhost:5672"))
    .AutoProvision();

    // 2. Tell the transport about the exchange type (Topic)
    rabbit.DeclareExchange("orders-exchange", exchange =>
    {
        exchange.ExchangeType = ExchangeType.Topic;
    });

    rabbit.DeclareQueue("delivery-service-inbox", queue =>
    {
        queue.BindExchange("orders-exchange", "order.#");
    });

    // 4. Setup the actual listener for that queue
    opts.ListenToRabbitQueue("delivery-service-inbox");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
