using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.Repository.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<TaskResult<ProductDto?>> CreateProductAsync(string name, decimal price);
    public Task<TaskResult<ProductDto?>> GetProductByIdAsync(Guid id);
    public Task<TaskResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
    public Task<TaskResult<ProductDto?>> UpdateProductAsync(ProductDto dto);
    public Task<TaskResult<bool>> DeleteProductByIdAsync(Guid id);
}