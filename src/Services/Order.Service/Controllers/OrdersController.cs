using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Service.Data;
using Order.Service.DTOs;
using Shared.Contracts;

namespace Order.Service.Controllers;
[ApiController]
[Route("/api/[controller]")]
public sealed class OrdersController
    (OrderDbContext context, 
    IPublishEndpoint publishEndpoint): ControllerBase 
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var order = new Models.Order { Id = Guid.NewGuid(), CustomerName = request.CustomerName, TotalAmount = request.TotalAmount };

        context.Orders.Add(order);
        await context.SaveChangesAsync();
        
        // TO-DO: Add Outbox pattern to ensure reliable event publishing in case of failures during the database transaction.
        // Broadcast the event to the Mesh
        await publishEndpoint.Publish<IOrderCreated>(new
        {
            OrderId = order.Id,
            order.CustomerName,
            order.TotalAmount,
            CreatedAt = order.CreatedAt
        });

        return Ok(new { order.Id, Message = "Order created and event published." });
    }
}