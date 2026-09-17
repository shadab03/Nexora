using Customer.Application.Abstractions;
using Customer.Application.Customers;
using Customer.Domain.Customers;

namespace Customer.UnitTests;

public sealed class CustomerHandlerTests
{
    [Fact]
    public async Task GetCustomer_returns_customer_when_it_exists()
    {
        var customer = CustomerEntity.Create(
            "Ada",
            "Lovelace",
            "ada@example.com",
            null);

        var handler = new GetCustomerHandler(
            new StubCustomerRepository(customer));

        var result = await handler.HandleAsync(
            new GetCustomerQuery(customer.Id));

        Assert.True(result.IsSuccess);
        Assert.Equal(customer.Id, result.Value.Id);
        Assert.Equal("Ada", result.Value.FirstName);
    }

    [Fact]
    public async Task AddAddress_returns_not_found_when_customer_is_missing()
    {
        var handler = new AddAddressHandler(
            new StubCustomerRepository(null),
            new StubUnitOfWork(),
            new AddAddressValidator());

        var result = await handler.HandleAsync(
            new AddAddressCommand(
                Guid.NewGuid(),
                "1 Main St",
                "London",
                "GB",
                "SW1A 1AA"));

        Assert.True(result.IsFailure);
        Assert.Equal("Customer.NotFound", result.Error?.Code);
    }

    private sealed class StubCustomerRepository
        : ICustomerRepository
    {
        private readonly CustomerEntity? _customer;

        public StubCustomerRepository(CustomerEntity? customer)
        {
            _customer = customer;
        }

        public Task<CustomerEntity?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _customer?.Id == id ? _customer : null);
        }

        public Task<CustomerEntity?> GetByIdWithAddressesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _customer?.Id == id ? _customer : null);
        }

        public Task<CustomerEntity?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _customer?.Email == email ? _customer : null);
        }

        public Task AddAsync(
            CustomerEntity customer,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class StubUnitOfWork
        : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1);
        }
    }
}
