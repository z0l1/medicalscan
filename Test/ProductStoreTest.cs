using medicalscan.Core.Entities;
using medicalscan.Repository.Store;

namespace Test;

public class Tests
{
    private AsyncMemoryProductStore GetStore()
    {
        return AsyncMemoryProductStore.Instance;
    }
    // or _store and _store = AsyncMemoryProductStore.Instance inside Setup()

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task AddProductsToStoreAndCheckIdIncrements()
    {
        var firstExpectedProduct = new Product
        {
            Id = 1,
            Name = "first",
            Price = 100
        };

        var secondExpectedProduct = new Product
        {
            Id = 1,
            Name = "first",
            Price = 100
        };

        var result1 = await GetStore().CreateProduct(firstExpectedProduct.Name, firstExpectedProduct.Price);
        var result2 = await GetStore().CreateProduct(secondExpectedProduct.Name, secondExpectedProduct.Price);

        Assert.That(result1.Data, Is.Not.EqualTo(null));
        Assert.That(result1.Error, Is.EqualTo(null));
        
        Assert.That(result1.Data?.Id, Is.EqualTo(firstExpectedProduct.Id));
        Assert.That(result1.Data?.Name, Is.EqualTo(firstExpectedProduct.Name));
        Assert.That(result1.Data?.Price, Is.EqualTo(firstExpectedProduct.Price));

        Assert.That(result2.Data, Is.Not.EqualTo(null));
        Assert.That(result2.Error, Is.EqualTo(null));
        
        Assert.That(result2.Data?.Id, Is.EqualTo(secondExpectedProduct.Id));
        Assert.That(result2.Data?.Name, Is.EqualTo(secondExpectedProduct.Name));
        Assert.That(result2.Data?.Price, Is.EqualTo(secondExpectedProduct.Price));
    }
}