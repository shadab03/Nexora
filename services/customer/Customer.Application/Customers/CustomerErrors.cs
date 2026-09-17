using Customer.Application.Common;

namespace Customer.Application.Customers;

public static class CustomerErrors
{
    public static readonly Error EmailAlreadyExists =
        new(
            "Customer.EmailAlreadyExists",
            "A customer with this email already exists.");

    public static readonly Error NotFound =
        new(
            "Customer.NotFound",
            "The customer was not found.");
}