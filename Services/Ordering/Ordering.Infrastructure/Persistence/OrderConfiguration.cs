using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasData
        (
            new Order
            {
                UserName = "plc",
                FirstName = "Truong Minh",
                LastName = "Phuoc",
                EmailAddress = "ht10082001@gmail.com",
                AddressLine = "DaNang",
                Country = "VietNam",
                TotalPrice = 27
            },
            new Order
            {
                UserName = "sj",
                FirstName = "Steve",
                LastName = "Job",
                EmailAddress = "stevejob@apple.com",
                AddressLine = "SanFrancisco",
                Country = "America",
                TotalPrice = 100
            }
        );
    }
}
