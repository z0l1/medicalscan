using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.DomainLogic.Services.Interfaces;

public interface IProductService
{
    public Task<HandledResponse<Product?>> CreateProductAsync(string name, decimal price);
    public Task<HandledResponse<Product?>> GetProductByIdAsync(long id);
    public Task<HandledResponse<IEnumerable<Product>>> GetAllProductsAsync();
    public Task<HandledResponse<Product?>> UpdateProductAsync(Product entity);
    public Task<HandledResponse<bool>> DeleteProductByIdAsync(long id);
}