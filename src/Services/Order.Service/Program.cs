using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Service.Data;
using Shared.Contracts;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseWolverine(opts =>
{
    var rabbit = opts.UseRabbitMq(new Uri("amqp://guest:guest@localhost:5672"))
    .AutoProvision();

    opts.PublishMessage<OrderCreatedEvent>()
        .ToRabbitRoutingKey("orders-exchange", "order.created", config =>
        {
            config.ExchangeType = ExchangeType.Topic;
        });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
