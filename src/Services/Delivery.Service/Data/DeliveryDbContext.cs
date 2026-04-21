using Microsoft.EntityFrameworkCore;

namespace Delivery.Service.Data;

public sealed class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<Models.DeliveryRecord> Deliveries => Set<Models.DeliveryRecord>();
}
