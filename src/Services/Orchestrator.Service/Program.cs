using Microsoft.EntityFrameworkCore;
using Orchestrator.Service.Data;
using Orchestrator.Service.Sagas;
using Shared.Contracts;
using Wolverine;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrchestratorDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Host.UseWolverine(opts =>
{
    // Setup RabbitMQ Connection
    var rabbit = opts.UseRabbitMq(new Uri("amqp://guest:guest@localhost:5672"))
                     .AutoProvision();

    // PERSISTENCE: Tell Wolverine to store Saga state in our DbContext
    opts.PersistMessagesWithPostgresql(connectionString);

    rabbit.DeclareExchange("orders-exchange", ex => ex.ExchangeType = ExchangeType.Topic);
    rabbit.DeclareExchange("delivery-exchange", ex => ex.ExchangeType = ExchangeType.Topic);

    // INBOUND: Listen to Order Service
    opts.ListenToRabbitQueue("orchestrator-orders-inbox", config =>
    {
        config.BindExchange("orders-exchange", "order.#");
    });

    // INBOUND: Listen to Delivery Service
    opts.ListenToRabbitQueue("orchestrator-delivery-inbox", config =>
    {
        config.BindExchange("delivery-exchange", "delivery.#");
    });

    // Ensure all DB changes and messages happen in a single transaction
    opts.Policies.AutoApplyTransactions();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Orchestrator (The Brain) is running...");

app.Run();