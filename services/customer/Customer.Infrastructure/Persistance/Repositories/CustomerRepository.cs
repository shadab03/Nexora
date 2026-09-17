using Customer.Application.Abstractions;
using Customer.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository
    : ICustomerRepository
{
    private readonly CustomerDbContext _dbContext;

    public CustomerRepository(
        CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CustomerEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<CustomerEntity?> GetByIdWithAddressesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<CustomerEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task AddAsync(
        CustomerEntity customer,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Customers.AddAsync(
            customer,
            cancellationToken);
    }
}
