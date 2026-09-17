namespace Customer.Api.Contracts.Customers;

public sealed record AddAddressRequest(
    string Line1,
    string City,
    string Country,
    string PostalCode);
