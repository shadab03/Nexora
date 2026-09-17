using Customer.Application.Customers.CreateCustomer;
using Customer.Application.Customers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);

        services.AddScoped<CreateCustomerHandler>();
        services.AddScoped<GetCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<AddAddressHandler>();
        services.AddScoped<GetCustomerAddressesHandler>();

        return services;
    }
}
