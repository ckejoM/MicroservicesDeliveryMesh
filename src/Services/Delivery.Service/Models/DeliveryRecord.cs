namespace Delivery.Service.Models;
public sealed class DeliveryRecord
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string CourierName { get; set; } = "Pending Assignment";
    public string Status { get; set; } = "InPreparation";
}