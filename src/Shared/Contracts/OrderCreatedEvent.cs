namespace Shared.Contracts;
public record OrderCreatedEvent (Guid OrderId, string CustomerName, decimal TotalAmount, DateTime CreatedAt);