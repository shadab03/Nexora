namespace Customer.Domain.Customers;

public sealed class Address
{
    private Address()
    {
    }

    internal Address(
        Guid id,
        string line1,
        string city,
        string country,
        string postalCode)
    {
        Id = id;
        Line1 = line1;
        City = city;
        Country = country;
        PostalCode = postalCode;
    }

    public Guid Id { get; private set; }

    public string Line1 { get; private set; }

    public string City { get; private set; }

    public string Country { get; private set; }

    public string PostalCode { get; private set; }
}