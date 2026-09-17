using Customer.Application.Abstractions;
using Customer.Infrastructure.Persistence;
using Customer.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("CustomerDatabase");

        services.AddDbContext<CustomerDbContext>(
            options =>
                options.UseNpgsql(connectionString));

        services.AddScoped<
            ICustomerRepository,
            CustomerRepository>();

        services.AddScoped<
            IUnitOfWork,
            UnitOfWork>();

        return services;
    }
}