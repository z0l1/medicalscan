using medicalscan.Core;
using medicalscan.Core.Entities;
using medicalscan.Repository.Repositories.Interfaces;
using medicalscan.Repository.Store;
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

    [HttpGet("/getAllProducts")]
    public async Task<HandledResponse<IEnumerable<Product>>> GetAllProducts()
    {
        var result = await _productRepository.GetAllProductsAsync();
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<IEnumerable<Product>>(500, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    [HttpGet("/getProductById/{id:long}")]
    public async Task<HandledResponse<Product?>> GetProductById(long id)
        // public async Task<HandledResponse<Product?>> GetProductById([FromQuery] long id)
    {
        var result = await _productRepository.GetProductByIdAsync(id);
        if (result.Error != null)
        {
            // if (result.Error == ProductErrors.CouldNotFind)
            return ResponseHandling.MakeStatusCodeResponse<Product?>(404, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    [HttpPost("/createProduct")]
    public async Task<HandledResponse<Product?>> CreateProduct([FromBody] Product productRequest)
    {
        var result = await _productRepository.CreateProductAsync(productRequest.Name, productRequest.Price);
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<Product?>(500, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    [HttpPut("/updateProduct")]
    public async Task<HandledResponse<Product>> UpdateProduct([FromBody] Product productRequest)
    {
        var result = await _productRepository.UpdateProductAsync(productRequest);
        if (result.Error != null)
        {
            int code = 400;
            if (result.Error == ProductErrors.CouldNotFind) code = 404;
            if (result.Error == ProductErrors.CouldNotUpdate) code = 500;
            
            return ResponseHandling.MakeStatusCodeResponse<Product>(code, null, result.Error);
        }
        
        return ResponseHandling.MakeOkResponse(result.Data);
    }

    [HttpDelete("/deleteById/{id:long}")]
    public async Task<HandledResponse<bool>> DeleteProductById(long id)
    {
        var result = await _productRepository.DeleteProductByIdAsync(id);
        if (result.Error != null)
        {
            int code = 400;
            if (result.Error == ProductErrors.CouldNotFind) code = 404;
            if (result.Error == ProductErrors.CouldNotDelete) code = 500;
            
            return ResponseHandling.MakeStatusCodeResponse<bool>(code, false, result.Error);
        }
        
        return ResponseHandling.MakeOkResponse(result.Data);
    }
}