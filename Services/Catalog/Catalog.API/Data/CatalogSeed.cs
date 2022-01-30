using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogSeed
{
    public static async void SeedData(IMongoCollection<Product> productCollection)
    {
        bool existProduct = productCollection.Find(_ => true).Any();
        if (!existProduct)
        {
            await productCollection.InsertManyAsync(ProductList());
        }
    }

    private static IEnumerable<Product> ProductList()
    {
        return new List<Product>()
        {
            new Product
            {
                Name = "iPhone 6s Plus",
                Category = "Apple",
                Description = "My Phone",
                Price = 10.08,
                Image = "ip-6sp"
            },
            new Product
            {
                Name = "Dell Inspiron 5567",
                Category = "Dell",
                Description = "My laptop",
                Price = 200.5,
                Image = "dell-5567"
            },
            new Product
            {
                Name = "LG Gram",
                Category = "LG",
                Description = "A nice laptop",
                Price = 100.7,
                Image = "lg-gram"
            },
            new Product
            {
                Name = "Macbook Air M1",
                Category = "Apple",
                Description = "Best choice",
                Price = 1000.69,
                Image = "mac-air"
            }
        };
    }
}
