namespace Shared.Contracts;

public interface IDeliveryAllocated
{
    Guid OrderId { get; }
    Guid DeliveryId { get; }
    string CourierName { get; }
    DateTime AllocatedAt { get; }
}
