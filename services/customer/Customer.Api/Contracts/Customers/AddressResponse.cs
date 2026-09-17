namespace Customer.Api.Contracts.Customers;

public sealed record AddressResponse(
    Guid Id,
    string Line1,
    string City,
    string Country,
    string PostalCode);
