namespace Orchestrator.Service.Models;

public sealed class OrderSaga
{
    // Wolverine uses this Id to correlate all incoming events
    public Guid Id { get; set; }
    public string Status { get; set; } = "Started";
    public string? CustomerName { get; set; }
    public string? CourierName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ShippedAt { get; set; }
}