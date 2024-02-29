using medicalscan.Core;
using medicalscan.Core.Entities;
using medicalscan.Repository.Repositories.Interfaces;
using medicalscan.Utils;
using Microsoft.AspNetCore.Mvc;

namespace medicalscan.Controllers;

[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<HandledResponse<IEnumerable<Product>>> GetAllProducts()
    {
        var result = await _productRepository.GetAllProductsAsync();
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<>(400, null, result.Error);
        }
        
        return ResponseHandling.MakeOkResponse(result.Data);
    }

    [HttpGet]
    public async Task<HandledResponse<Product?>> GetProductById(long id)
    {
        // return ResponseHandling.MakeOkResponse(new Product { Id = 0, Name = "asd", Price = 1500 });
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<HandledResponse<Product>> CreateProduct([FromBody] Product productRequest)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public async Task<HandledResponse<Product>> UpdateProduct([FromBody] Product productRequest)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<HandledResponse<object>> DeleteProductById(long id)
    {
        throw new NotImplementedException();
    }
}