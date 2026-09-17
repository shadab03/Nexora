namespace Customer.Domain.Customers;

public sealed class CustomerEntity
{
    private readonly List<Address> _addresses = [];

    private CustomerEntity()
    {
        // Required by EF Core
    }

    private CustomerEntity(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string? phoneNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? PhoneNumber { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Address> Addresses =>
        _addresses.AsReadOnly();

    public static CustomerEntity Create(
        string firstName,
        string lastName,
        string email,
        string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        return new CustomerEntity(
            Guid.NewGuid(),
            firstName.Trim(),
            lastName.Trim(),
            email.Trim().ToLowerInvariant(),
            phoneNumber?.Trim());
    }

    public void Update(
        string firstName,
        string lastName,
        string email,
        string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PhoneNumber = phoneNumber?.Trim();
    }

    public Address AddAddress(
        string line1,
        string city,
        string country,
        string postalCode)
    {
        if (string.IsNullOrWhiteSpace(line1))
            throw new ArgumentException("Address line 1 is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code is required.");

        var address = new Address(
            Guid.NewGuid(),
            line1.Trim(),
            city.Trim(),
            country.Trim(),
            postalCode.Trim());

        _addresses.Add(address);

        return address;
    }
}
