using System.Collections.Concurrent;
using MedicalScan.Core.Entities;

namespace MedicalScan.Repository.Store;

public class AsyncMemoryProductStore
{
    private static AsyncMemoryProductStore _instance;
    private static readonly object _lock = new object();

    private readonly object _idLock = new object();
    private long _nextId = 1;
    private ConcurrentDictionary<long, Product> _productDict = new ConcurrentDictionary<long, Product>();

    private AsyncMemoryProductStore()
    {
    }

    private long GetNextId()
    {
        long id = -1;

        lock (_idLock)
        {
            id = _nextId++;
        }

        return id;
    }

    public static AsyncMemoryProductStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AsyncMemoryProductStore();
                    }
                }
            }

            return _instance;
        }
    }

    public async Task<TaskResult<Product?>> CreateProduct(string name, decimal price)
    {
        var id = GetNextId();

        var entity = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        var result = _productDict.TryAdd(id, entity);
        if (!result)
        {
            return new TaskResult<Product?>(null, ProductErrors.CouldNotCreate);
        }

        return new TaskResult<Product?>(entity);
    }

    public async Task<TaskResult<Product?>> GetProductById(long id)
    {
        var result = _productDict.TryGetValue(id, out var product);
        if (!result)
        {
            return new TaskResult<Product?>(null, ProductErrors.CouldNotFind);
        }

        return new TaskResult<Product?>(product);
    }

    public async Task<TaskResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = _productDict.Values.ToList().AsEnumerable();
        return new TaskResult<IEnumerable<Product>>(products);
    }

    public async Task<TaskResult<Product?>> UpdateProduct(Product entity)
    {
        var productResult = await GetProductById(entity.Id);
        if (productResult.Error != null)
        {
            return new TaskResult<Product?>(null, productResult.Error);
        }

        // should never happen, except if "someone" forgot to return data in getproductbyid
        if (productResult.Data?.Id != entity.Id)
        {
            return new TaskResult<Product?>(null,
                $"entity id ({entity.Id}) does not match with key ({productResult.Data?.Id})");
        }

        var result = _productDict.TryUpdate(entity.Id, entity, productResult.Data);
        if (!result)
        {
            return new TaskResult<Product?>(null, ProductErrors.CouldNotUpdate);
        }

        return new TaskResult<Product?>(entity);
    }

    public async Task<TaskResult<bool>> DeleteProductById(long id)
    {
        var productResult = await GetProductById(id);
        if (productResult.Error != null)
        {
            return new TaskResult<bool>(false, productResult.Error);
        }

        var result = _productDict.TryRemove(id, out var removedProduct);
        var err = result ? null : ProductErrors.CouldNotDelete;

        return new TaskResult<bool>(result, err);
    }
}