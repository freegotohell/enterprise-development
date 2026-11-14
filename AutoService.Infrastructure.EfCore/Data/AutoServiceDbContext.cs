using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Data;

public class AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<WorkType> WorkTypes => Set<WorkType>();
    public DbSet<RepairOrder> RepairOrders => Set<RepairOrder>();
    public DbSet<OrderMechanic> OrderMechanics => Set<OrderMechanic>();
    public DbSet<OrderWork> OrderWorks => Set<OrderWork>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoServiceDbContext).Assembly);
    }
}
