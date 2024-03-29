﻿using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.DomainLogic.Services.Interfaces;
using MedicalScanBackend.DomainLogic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScanBackend.Controllers;

[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/getAllProducts")]
    public async Task<HandledResponse<IEnumerable<ProductDto>>> GetAllProducts()
    {
        return await _productService.GetAllProductsAsync();
    }

    [HttpGet("/getProductById/{id:Guid}")]
    public async Task<HandledResponse<ProductDto?>> GetProductById(Guid id)
        // public async Task<HandledResponse<Product?>> GetProductById([FromQuery] Guid id)
    {
        return await _productService.GetProductByIdAsync(id);
    }

    [HttpPost("/createProduct")]
    public async Task<HandledResponse<ProductDto?>> CreateProduct([FromBody] CreateProductRequest createRequest)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            return ResponseHandling.MakeBadRequestResponse<ProductDto?>(string.Join(" ", errors));
        }
        
        return await _productService.CreateProductAsync(createRequest);
    }

    [HttpPut("/updateProduct")]
    public async Task<HandledResponse<ProductDto?>> UpdateProduct([FromBody] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            return ResponseHandling.MakeBadRequestResponse<ProductDto?>(string.Join(" ", errors));
        }
        return await _productService.UpdateProductAsync(productDto);
    }

    [HttpDelete("/deleteById/{id:Guid}")]
    public async Task<HandledResponse<bool>> DeleteProductById(Guid id)
    {
        return await _productService.DeleteProductByIdAsync(id);
    }
}