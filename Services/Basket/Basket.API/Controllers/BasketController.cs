using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.GrpcService;
using Discount.Grpc.Protos;
using Basket.API.Entities;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;
    public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
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
        // Gọi sang Discount Grpc để tính Final Price
        // Áp coupon cho từng sản phẩm
        foreach (ShoppingCartItem item in basket.Items)
        {
            CouponModel coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        return await _repository.UpdateBasket(basket);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    public async Task<IActionResult> DeleteBasket(string username)
    {
        await _repository.DeleteBasket(username);
        return NoContent();
    }
}
