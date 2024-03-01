using System.Collections.Concurrent;
using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.Repository.Store;

public class AsyncMemoryProductStore
{
    private static AsyncMemoryProductStore _instance;
    private static readonly object _lock = new object();

    private readonly object _idLock = new object();
    private long _nextId = 1;
    private ConcurrentDictionary<Guid, Product> _productDict = new ConcurrentDictionary<Guid, Product>();

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
        var uuid = Guid.NewGuid();

        var entity = new Product
        {
            Uuid = uuid,
            Id = id,
            Name = name,
            Price = price
        };

        var result = _productDict.TryAdd(uuid, entity);
        if (!result)
        {
            return new TaskResult<Product?>(null, ProductErrors.CouldNotCreate);
        }

        return new TaskResult<Product?>(entity);
    }

    public async Task<TaskResult<Product?>> GetProductById(Guid id)
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

    public async Task<TaskResult<Product?>> UpdateProduct(Guid id, string name, decimal price)
    {
        var productResult = await GetProductById(id);
        if (productResult.Error != null)
        {
            return new TaskResult<Product?>(null, productResult.Error);
        }

        // should never happen, except if "someone" forgot to return data in getproductbyid
        if (productResult.Data?.Uuid != id)
        {
            return new TaskResult<Product?>(null,
                $"entity id ({id}) does not match with key ({productResult.Data?.Uuid})");
        }

        var newProduct = new Product
        {
            Uuid = id,
            Id = productResult.Data.Id,
            Name = productResult.Data.Name,
            Price = productResult.Data.Price
        };

        var result = _productDict.TryUpdate(id, newProduct, productResult.Data);
        if (!result)
        {
            return new TaskResult<Product?>(null, ProductErrors.CouldNotUpdate);
        }

        return new TaskResult<Product?>(newProduct);
    }

    public async Task<TaskResult<bool>> DeleteProductById(Guid id)
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