using medicalscan.Core.Entities;
using medicalscan.Repository.Repositories.Interfaces;
using medicalscan.Repository.Store;

namespace medicalscan.Repository.Repositories;

public class ProductRepository: IProductRepository
{
    private AsyncMemoryProductStore GetStore()
    {
        return AsyncMemoryProductStore.Instance;
    }
    
    public Task<TaskResult<Product?>> CreateProductAsync(string name, decimal price)
    {
        return GetStore().CreateProduct(name, price);
    }

    public Task<TaskResult<Product?>> GetProductByIdAsync(long id)
    {
        return GetStore().GetProductById(id);
    }

    public Task<TaskResult<IEnumerable<Product>>> GetAllProductsAsync()
    {
        return GetStore().GetAllProducts();
    }

    public Task<TaskResult<Product?>> UpdateProductAsync(Product entity)
    {
        return GetStore().UpdateProduct(entity);
    }

    public Task<TaskResult<bool>> DeleteProductByIdAsync(long id)
    {
        return GetStore().DeleteProductById(id);
    }
}