using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.Repository.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<TaskResult<Product?>> CreateProductAsync(string name, decimal price);
    public Task<TaskResult<Product?>> GetProductByIdAsync(long id);
    public Task<TaskResult<IEnumerable<Product>>> GetAllProductsAsync();
    public Task<TaskResult<Product?>> UpdateProductAsync(Product entity);
    public Task<TaskResult<bool>> DeleteProductByIdAsync(long id);
}