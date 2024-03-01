using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.DomainLogic.Services.Interfaces;
using MedicalScanBackend.DomainLogic.Utils;
using MedicalScanBackend.Repository.Repositories.Interfaces;
using MedicalScanBackend.Repository.Store;

namespace MedicalScanBackend.DomainLogic.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<HandledResponse<ProductDto?>> CreateProductAsync(CreateProductRequest request)
    {
        var result = await _productRepository.CreateProductAsync(request.Name, request.Price);
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<ProductDto?>(500, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    public async Task<HandledResponse<ProductDto?>> GetProductByIdAsync(Guid id)
    {
        var result = await _productRepository.GetProductByIdAsync(id);
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<ProductDto?>(404, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    public async Task<HandledResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var result = await _productRepository.GetAllProductsAsync();
        if (result.Error != null)
        {
            return ResponseHandling.MakeStatusCodeResponse<IEnumerable<ProductDto>>(500, null, result.Error);
        }

        return ResponseHandling.MakeOkResponse(result.Data);
    }

    public async Task<HandledResponse<ProductDto?>> UpdateProductAsync(ProductDto dto)
    {
        var result = await _productRepository.UpdateProductAsync(dto);
        if (result.Error != null)
        {
            var code = 400;
            if (result.Error == ProductErrors.CouldNotFind) code = 404; 
            else if (result.Error == ProductErrors.CouldNotUpdate) code = 500;
            
            return ResponseHandling.MakeStatusCodeResponse<ProductDto>(code, null, result.Error);
        }
        
        return ResponseHandling.MakeOkResponse(result.Data);
    }

    public async Task<HandledResponse<bool>> DeleteProductByIdAsync(Guid id)
    {
        var result = await _productRepository.DeleteProductByIdAsync(id);
        if (result.Error != null)
        {
            var code = 400;
            if (result.Error == ProductErrors.CouldNotFind) code = 404;
            else if (result.Error == ProductErrors.CouldNotDelete) code = 500;
            
            return ResponseHandling.MakeStatusCodeResponse<bool>(code, false, result.Error);
        }
        
        return ResponseHandling.MakeOkResponse(result.Data);
    }
}