using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Entities;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{username}", Name = "GetBasket")]
    public async Task<ShoppingCart> GetBasket(string username)
    {
        ShoppingCart basket = await _repository.GetBasket(username);
        return basket ?? new ShoppingCart { UserName = username };
    }

    [HttpPost]
    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        return await _repository.UpdateBasket(basket);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    public async Task<IActionResult> DeleteBasket(string username)
    {
        await _repository.DeleteBasket(username);
        return NoContent();
    }
}
