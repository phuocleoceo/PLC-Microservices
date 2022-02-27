using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetSeedOrder());
            await orderContext.SaveChangesAsync();
            logger.LogInformation($"Seed database for {typeof(OrderContext).Name}");
        }
    }

    private static IEnumerable<Order> GetSeedOrder()
    {
        return new List<Order>
        {
            new Order
            {
                UserName="plc",
                FirstName="Truong Minh",
                LastName="Phuoc",
                EmailAddress="ht10082001@gmail.com",
                AddressLine="DaNang",
                Country="VietNam",
                TotalPrice=27
            },
            new Order
            {
                UserName="sj",
                FirstName="Steve",
                LastName="Job",
                EmailAddress="stevejob@apple.com",
                AddressLine="SanFrancisco",
                Country="America",
                TotalPrice=100
            }
        };
    }
}
