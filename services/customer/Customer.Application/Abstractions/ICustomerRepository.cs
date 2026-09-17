using Customer.Domain.Customers;

namespace Customer.Application.Abstractions;

public interface ICustomerRepository
{
    Task<CustomerEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<CustomerEntity?> GetByIdWithAddressesAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<CustomerEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CustomerEntity customer,
        CancellationToken cancellationToken = default);
}
