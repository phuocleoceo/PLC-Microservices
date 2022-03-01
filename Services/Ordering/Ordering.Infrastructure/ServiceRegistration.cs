using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Mail;
using Ordering.Application.Models;

namespace Ordering.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureService
                (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderCNS")));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.Configure<EmailSettings>(_ => configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
