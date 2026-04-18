namespace Shared.Contracts;
public interface IOrderCreated
{
    Guid OrderId { get; }
    string CustomerName { get; }
    decimal TotalAmount { get; }
    DateTime CreatedAt { get; }
}