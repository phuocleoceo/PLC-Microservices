using Discount.Grpc.Entities;
using Npgsql;
using Dapper;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    public readonly string CNS;
    public DiscountRepository(IConfiguration configuration)
    {
        CNS = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var con = new NpgsqlConnection(CNS);
        string query = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
        var parameters = new { ProductName = productName };
        Coupon coupon = await con.QueryFirstOrDefaultAsync<Coupon>(query, parameters);
        if (coupon is null)
        {
            return new Coupon { ProductName = "No Discount", Description = "No Desc", Amount = 0 };
        }
        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var con = new NpgsqlConnection(CNS);
        string query = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES
                        (@ProductName, @Description, @Amount);";
        var parameters = new
        {
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };
        return await con.ExecuteAsync(query, parameters) > 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var con = new NpgsqlConnection(CNS);
        string query = @"UPDATE Coupon SET ProductName = @ProductName, Description = @Description,
                        Amount = @Amount WHERE Id = @Id";
        var parameters = new
        {
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount,
            Id = coupon.Id
        };
        return await con.ExecuteAsync(query, parameters) > 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var con = new NpgsqlConnection(CNS);
        string query = "DELETE FROM Coupon WHERE ProductName = @ProductName";
        var parameters = new { ProductName = productName };
        return await con.ExecuteAsync(query, parameters) > 0;
    }
}
