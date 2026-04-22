using Microsoft.EntityFrameworkCore;
using Orchestrator.Service.Models;

namespace Orchestrator.Service.Data;

public sealed class OrchestratorDbContext(DbContextOptions<OrchestratorDbContext> options) : DbContext(options)
{
    public DbSet<Models.OrderSaga> OrderSagas => Set<OrderSaga>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure the Saga ID is the primary key
        modelBuilder.Entity<OrderSaga>().HasKey(x => x.Id);
    }
}
