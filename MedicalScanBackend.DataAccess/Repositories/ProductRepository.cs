using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Entities;
using MedicalScanBackend.Core.Mappers;
using MedicalScanBackend.Repository.Repositories.Interfaces;
using MedicalScanBackend.Repository.Store;

namespace MedicalScanBackend.Repository.Repositories;

public class ProductRepository : IProductRepository
{
    private AsyncMemoryProductStore GetStore()
    {
        return AsyncMemoryProductStore.Instance;
    }

    public async Task<TaskResult<ProductDto?>> CreateProductAsync(string name, decimal price)
    {
        var result = await GetStore().CreateProduct(name, price);
        return new TaskResult<ProductDto?>
        {
            Data = result.Data == null ? null : ProductMapper.EntityToDto(result.Data),
            Error = result.Error
        };
    }

    public async Task<TaskResult<ProductDto?>> GetProductByIdAsync(Guid id)
    {
        var result = await GetStore().GetProductById(id);
        return new TaskResult<ProductDto?>
        {
            Data = result.Data == null ? null : ProductMapper.EntityToDto(result.Data),
            Error = result.Error
        };
    }

    public async Task<TaskResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var result = await GetStore().GetAllProducts();
        var mappedData = result.Data?
            .Select(entity => ProductMapper.EntityToDto(entity));

        return new TaskResult<IEnumerable<ProductDto>>
        {
            Data = mappedData,
            Error = result.Error
        };
    }

    public async Task<TaskResult<ProductDto?>> UpdateProductAsync(ProductDto dto)
    {
        var result = await GetStore().UpdateProduct(dto.Id, dto.Name, dto.Price);
        return new TaskResult<ProductDto?>
        {
            Data = result.Data == null ? null : ProductMapper.EntityToDto(result.Data),
            Error = result.Error
        };
    }

    public Task<TaskResult<bool>> DeleteProductByIdAsync(Guid id)
    {
        return GetStore().DeleteProductById(id);
    }
}