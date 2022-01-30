using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        string CNS = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        string DB = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
        string CL = configuration.GetValue<string>("DatabaseSettings:CollectionName");

        MongoClient mongoClient = new MongoClient(CNS);
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(DB);
        Products = mongoDatabase.GetCollection<Product>(CL);

        CatalogSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
