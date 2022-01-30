using Catalog.API.Entities;
using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _db;
    public ProductRepository(ICatalogContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _db.Products.Find(_ => true).ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _db.Products.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        return await _db.Products.Find(c => c.Name == name).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string category)
    {
        return await _db.Products.Find(c => c.Category == category).ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _db.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        ReplaceOneResult updateResult = await _db.Products
                        .ReplaceOneAsync(filter: c => c.Id == product.Id, replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        DeleteResult deleteResult = await _db.Products.DeleteOneAsync(c => c.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
