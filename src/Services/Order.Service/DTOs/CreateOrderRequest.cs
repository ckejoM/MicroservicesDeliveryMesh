namespace Order.Service.DTOs;

public sealed record CreateOrderRequest(string CustomerName, decimal TotalAmount);