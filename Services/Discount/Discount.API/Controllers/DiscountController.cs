using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Discount.API.Entities;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;
    public DiscountController(IDiscountRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{productName}", Name = "GetDiscount")]
    public async Task<Coupon> GetDiscount(string productName)
    {
        return await _repository.GetDiscount(productName);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount(Coupon coupon)
    {
        if (ModelState.IsValid && await _repository.CreateDiscount(coupon))
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        else
            return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDiscount(Coupon coupon)
    {
        if (ModelState.IsValid && await _repository.UpdateDiscount(coupon))
            return NoContent();
        else
            return BadRequest();
    }

    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    public async Task<IActionResult> DeleteDiscount(string productName)
    {
        if (await _repository.DeleteDiscount(productName))
            return NoContent();
        else
            return BadRequest();
    }
}


