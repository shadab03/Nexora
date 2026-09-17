using Customer.Domain.Customers;

namespace Customer.UnitTests;

public sealed class CustomerEntityTests
{
    [Fact]
    public void Update_changes_customer_profile_details()
    {
        var customer = CustomerEntity.Create(
            "Ada",
            "Lovelace",
            "ada@example.com",
            "123");

        customer.Update(
            "Grace",
            "Hopper",
            "GRACE@EXAMPLE.COM",
            "456");

        Assert.Equal("Grace", customer.FirstName);
        Assert.Equal("Hopper", customer.LastName);
        Assert.Equal("grace@example.com", customer.Email);
        Assert.Equal("456", customer.PhoneNumber);
    }

    [Fact]
    public void AddAddress_adds_trimmed_address_to_customer()
    {
        var customer = CustomerEntity.Create(
            "Ada",
            "Lovelace",
            "ada@example.com",
            null);

        var address = customer.AddAddress(
            "  1 Main St  ",
            "  London ",
            " GB ",
            " SW1A 1AA ");

        Assert.Contains(address, customer.Addresses);
        Assert.Equal("1 Main St", address.Line1);
        Assert.Equal("London", address.City);
        Assert.Equal("GB", address.Country);
        Assert.Equal("SW1A 1AA", address.PostalCode);
    }
}
