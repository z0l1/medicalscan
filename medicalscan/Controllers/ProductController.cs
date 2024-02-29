using medicalscan.Core;
using medicalscan.Core.Entities;
using medicalscan.Utils;
using Microsoft.AspNetCore.Mvc;

namespace medicalscan.Controllers;

[Route("[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public async Task<HandledResponse<IEnumerable<Product>>> GetAllProducts()
    {
        var items = new List<Product>() { new() { Id = 0, Name = "asd", Price = 1500 } }.AsEnumerable();
        return ResponseHandling.MakeOkResponse(items);
    }

    [HttpGet]
    public async Task<HandledResponse<Product?>> GetProductById(int id)
    {
        // return ResponseHandling.MakeOkResponse(new Product { Id = 0, Name = "asd", Price = 1500 });
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<HandledResponse<Product>> CreateProduct([FromBody] Product productRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<HandledResponse<Product>> UpdateProduct([FromBody] Product productRequest)
    {
        throw new NotImplementedException();
    }
}