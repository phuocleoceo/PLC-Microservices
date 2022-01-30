using Microsoft.AspNetCore.Mvc;
using Catalog.API.Repositories;
using Catalog.API.Entities;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<CatalogController> _logger;
    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _repository.GetAllProducts();
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        Product product = await _repository.GetProduct(id);
        if (product is null)
        {
            _logger.LogError($"Product has id : {id} is not found !");
            return NotFound();
        }
        return product;
    }

    [Route("[action]/{name}", Name = "GetProductByName")]
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        IEnumerable<Product> products = await _repository.GetProductByName(name);
        return products;
    }

    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProductByCategory(string category)
    {
        IEnumerable<Product> products = await _repository.GetProductByCategory(category);
        return products;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            await _repository.CreateProduct(product);

            _logger.LogInformation($"Created product with id {product.Id}");
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            Product prd = await _repository.GetProduct(product.Id);
            if (prd is null)
            {
                _logger.LogError($"Product has id : {product.Id} is not found to update");
                return NotFound();
            }
            if (await _repository.UpdateProduct(product))
            {
                _logger.LogInformation($"Update product {product.Id} success");
                return NoContent();
            }
            else
            {
                _logger.LogError($"Update product {product.Id} fail");
                return BadRequest();
            }
        }
        return BadRequest();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        Product prd = await _repository.GetProduct(id);
        if (prd is null)
        {
            _logger.LogError($"Product has id : {id} is not found to delete");
            return NotFound();
        }
        if (await _repository.DeleteProduct(id))
        {
            _logger.LogInformation($"Delete product {id} success");
            return NoContent();
        }
        else
        {
            _logger.LogError($"Delete product {id} fail");
            return BadRequest();
        }
    }
}
