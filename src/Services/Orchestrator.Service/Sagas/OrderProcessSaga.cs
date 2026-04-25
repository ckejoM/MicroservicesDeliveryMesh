using Orchestrator.Service.Models;
using Shared.Contracts;
using Wolverine;
using Wolverine.Persistence.Sagas;

namespace Orchestrator.Service.Sagas;

public class OrderProcessSaga : Saga
{
    public Guid Id { get; set; }
    // 1. START: Triggered by Order Service
    // Static Start methods tell Wolverine to create a NEW state record
    public static OrderSaga Start([SagaIdentityFrom(nameof(OrderCreatedEvent.OrderId))] OrderCreatedEvent message, ILogger<OrderProcessSaga> logger)
    {
        logger.LogInformation("--- SAGA START: Order {OrderId} detected ---", message.OrderId);

        return new OrderSaga
        {
            Id = message.OrderId,
            CustomerName = message.CustomerName,
            TotalAmount = message.TotalAmount,
            Status = "AwaitingDelivery"
        };
    }

    // 2. UPDATE: Triggered by Delivery Service
    // Instance methods tell Wolverine to find the existing state by Id and update it
    public void Handle([SagaIdentityFrom(nameof(OrderCreatedEvent.OrderId))] DeliveryAllocatedEvent message, OrderSaga state, ILogger<OrderProcessSaga> logger)
    {
        logger.LogInformation("--- SAGA UPDATE: Delivery confirmed for {OrderId} ---", message.OrderId);

        state.Status = "Shipped";
        state.CourierName = message.CourierName;
        state.ShippedAt = DateTime.UtcNow;

        logger.LogInformation("Order {OrderId} is now SHIPPED via {Courier}", state.Id, state.CourierName);

        // Final Step: In a real app, you might call 'MarkCompleted()' here 
        // if you want to delete the row after finishing the workflow.
    }
}
