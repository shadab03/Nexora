using Customer.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Persistence;

public sealed class CustomerDbContext : DbContext
{
    public CustomerDbContext(
        DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers =>
        Set<CustomerEntity>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CustomerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}