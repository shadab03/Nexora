using Customer.Application.Abstractions;
using Customer.Application.Common;

namespace Customer.Application.Customers;

public sealed record GetCustomerQuery(
    Guid CustomerId);

public sealed record CustomerResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime CreatedAt);

public sealed class GetCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler(
        ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerResult>> HandleAsync(
        GetCustomerQuery query,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _customerRepository.GetByIdAsync(
                query.CustomerId,
                cancellationToken);

        if (customer is null)
        {
            return Result<CustomerResult>.Failure(
                CustomerErrors.NotFound);
        }

        return Result<CustomerResult>.Success(
            new CustomerResult(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                customer.CreatedAt));
    }
}
