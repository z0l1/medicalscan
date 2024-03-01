using MedicalScanBackend.Core.DTOs;

namespace MedicalScanBackend.DomainLogic.Services.Interfaces;

public interface IProductService
{
    public Task<HandledResponse<ProductDto?>> CreateProductAsync(CreateProductRequest request);
    public Task<HandledResponse<ProductDto?>> GetProductByIdAsync(Guid id);
    public Task<HandledResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
    public Task<HandledResponse<ProductDto?>> UpdateProductAsync(ProductDto dto);
    public Task<HandledResponse<bool>> DeleteProductByIdAsync(Guid id);
}