namespace Shared.Contracts;

public record DeliveryAllocatedEvent(
    Guid OrderId,
    string CourierName,
    DateTime AllocatedAt
);