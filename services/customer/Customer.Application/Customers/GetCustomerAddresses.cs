using Customer.Application.Abstractions;
using Customer.Application.Common;

namespace Customer.Application.Customers;

public sealed record GetCustomerAddressesQuery(
    Guid CustomerId);

public sealed class GetCustomerAddressesHandler
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerAddressesHandler(
        ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<IReadOnlyCollection<AddressResult>>> HandleAsync(
        GetCustomerAddressesQuery query,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _customerRepository.GetByIdWithAddressesAsync(
                query.CustomerId,
                cancellationToken);

        if (customer is null)
        {
            return Result<IReadOnlyCollection<AddressResult>>.Failure(
                CustomerErrors.NotFound);
        }

        var addresses = customer.Addresses
            .Select(x => new AddressResult(
                x.Id,
                x.Line1,
                x.City,
                x.Country,
                x.PostalCode))
            .ToArray();

        return Result<IReadOnlyCollection<AddressResult>>.Success(
            addresses);
    }
}
