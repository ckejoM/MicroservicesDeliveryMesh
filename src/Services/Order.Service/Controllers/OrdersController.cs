using Microsoft.AspNetCore.Mvc;
using Order.Service.Data;
using Order.Service.DTOs;
using Shared.Contracts;
using Wolverine;

namespace Order.Service.Controllers;
[ApiController]
[Route("/api/[controller]")]
public sealed class OrdersController
    (OrderDbContext context, 
    ILogger<OrdersController> logger,
    IMessageBus bus): ControllerBase 
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var order = new Models.Order { Id = Guid.NewGuid(), CustomerName = request.CustomerName, TotalAmount = request.TotalAmount };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // TO-DO: Add Outbox pattern to ensure reliable event publishing in case of failures during the database transaction.
        // Broadcast the event to the Mesh
        // Wolverine Publishing
        logger.LogInformation("Publishing message {customerName}", order.CustomerName);

        await bus.PublishAsync(new OrderCreatedEvent
        (
            order.Id,
            order.CustomerName,
            order.TotalAmount,
            DateTime.UtcNow
        ));

        return Ok(new { order.Id, Message = "Order created and event published." });
    }
}