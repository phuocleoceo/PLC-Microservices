using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Entities;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasket(string username)
    {
        // Get bằng Key
        string basket = await _redisCache.GetStringAsync(username);
        if (String.IsNullOrEmpty(basket)) return null;
        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        // Set lại Key-Value
        string updateBasket = JsonConvert.SerializeObject(basket);
        await _redisCache.SetStringAsync(basket.UserName, updateBasket);
        return await GetBasket(basket.UserName);
    }

    public async Task DeleteBasket(string username)
    {
        await _redisCache.RemoveAsync(username);
    }
}
