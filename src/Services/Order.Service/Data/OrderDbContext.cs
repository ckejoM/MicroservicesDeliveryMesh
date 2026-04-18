using Microsoft.EntityFrameworkCore;

namespace Order.Service.Data;

public sealed class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Models.Order> Orders => Set<Models.Order>();
}
