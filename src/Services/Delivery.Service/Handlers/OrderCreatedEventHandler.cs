using Delivery.Service.Data;
using Delivery.Service.Models;
using Shared.Contracts;

namespace Delivery.Service.Handlers;
public sealed class OrderCreatedEventHandler
    (DeliveryDbContext dbContext,
    ILogger<OrderCreatedEventHandler> logger)
{
    public async Task Handle(OrderCreatedEvent orderCreatedEvent)
    {
        logger.LogInformation("Order Received: {OrderId} for {Customer}", orderCreatedEvent.OrderId, orderCreatedEvent.CustomerName);

        // Business Logic: Assign a courier (Simulator)
        var delivery = new DeliveryRecord
        {
            Id = Guid.NewGuid(),
            OrderId = orderCreatedEvent.OrderId,
            CourierName = "Courier_Express_" + Random.Shared.Next(1, 100),
            Status = "Allocated"
        };

        // TO-DO: Check if the order already has a delivery record to avoid duplicates (idempotency)

        dbContext.Deliveries.Add(delivery);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Delivery allocated for Order {OrderId}", orderCreatedEvent.OrderId);

        // In the next commit, we will publish IDeliveryAllocated here!
    }
}
