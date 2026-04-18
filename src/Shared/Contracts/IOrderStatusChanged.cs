namespace Shared.Contracts;

public interface IOrderStatusChanged
{
    Guid OrderId { get; }
    string NewStatus { get; } // e.g., "Pending", "Allocated", "Shipped"
    DateTime UpdatedAt { get; }
}
