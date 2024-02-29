using System.Collections.Concurrent;
using medicalscan.Core.Entities;

namespace medicalscan.Repository.Store;

public class MemoryProductStore
{
    private static MemoryProductStore _instance;
    private static readonly object _lock = new object();

    private readonly object _idLock = new object();
    private long _nextId = 1;
    private ConcurrentDictionary<long, Product> _productDict = new ConcurrentDictionary<long, Product>();

    private MemoryProductStore()
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

    public static MemoryProductStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MemoryProductStore();
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
            return new TaskResult<Product?>(null, "could not create product");
        }

        return new TaskResult<Product?>( entity);
    }

    public async Task<TaskResult< Product?>> GetProductById(long id)
    {
        var result = _productDict.TryGetValue(id, out var product);
        if (!result)
        {
            return new TaskResult<Product?>(null, $"product with id ({id}) does not exist");
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
            return new TaskResult<Product?>(null,productResult.Error, "error updating product");
        }

        // should never happen, except if "someone" forgot to return data in getproductbyid
        if (productResult.Data?.Id != entity.Id)
        {
            return new TaskResult<Product?>(null, $"entity id ({entity.Id}) does not match with key ({productResult.Data?.Id})");
        }

        var result = _productDict.TryUpdate(entity.Id, entity, productResult.Data);
        if (!result)
        {
            return new TaskResult<Product?>(null, $"could not update product ({entity.Id})");
        }

        return new TaskResult<Product?>(entity);
    }

    public async Task<TaskResult<bool>> DeleteProductById(long id)
    {
        var productResult = await GetProductById(id);
        if (productResult.Error != null)
        {
            return new TaskResult<bool>(false, productResult.Error, "error deleting product");
        }
        
        var result = _productDict.TryRemove(id, out var removedProduct);
        var err = result ? null : "could not deleting product";

        return new TaskResult<bool>(result, err);
    }
}