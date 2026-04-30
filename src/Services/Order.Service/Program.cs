using Microsoft.EntityFrameworkCore;
using Order.Service.Data;
using Serilog;
using Shared.Contracts;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

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
