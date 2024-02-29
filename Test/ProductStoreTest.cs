using medicalscan.Core.Entities;
using medicalscan.Repository.Store;

namespace Test;

public class Tests
{
    private AsyncMemoryProductStore GetStore()
    {
        return AsyncMemoryProductStore.Instance;
    }

    [SetUp]
    public async Task Setup()
    {
    }

    // I read it is not recommended, also tests should not be dependent on each other (interdependent)
    // for demo purposes since Im dealing with a singleton I just want to test getting empty list
    [Order(0)]
    [Test]
    public async Task GetAllTest()
    {
        var result0 = await GetStore().GetAllProducts();
        Assert.That(result0.Error, Is.Null);
        Assert.That(result0.Data, Is.Not.Null);
        Assert.That(result0.Data?.Count(), Is.EqualTo(0));

        for (int i = 1; i <= 10; i++)
        {
            await GetStore().CreateProduct($"item-{i}", i * 100);
        }

        var result1 = await GetStore().GetAllProducts();

        Assert.That(result1.Error, Is.Null);
        Assert.That(result1.Data, Is.Not.Null);
        Assert.That(result1.Data?.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task AddProductsToStoreAndCheckIdIncrementsTest()
    {
        var firstExpectedProduct = new Product
        {
            Id = 1,
            Name = "first",
            Price = 100
        };

        var secondExpectedProduct = new Product
        {
            Id = 2,
            Name = "second",
            Price = 200
        };

        var result1 = await GetStore().CreateProduct(firstExpectedProduct.Name, firstExpectedProduct.Price);
        var result2 = await GetStore().CreateProduct(secondExpectedProduct.Name, secondExpectedProduct.Price);

        Assert.That(result1.Data, Is.Not.Null);
        Assert.That(result1.Error, Is.Null);

        // Assert.That(result1.Data?.Id, Is.EqualTo(firstExpectedProduct.Id));
        Assert.That(result1.Data?.Name, Is.EqualTo(firstExpectedProduct.Name));
        Assert.That(result1.Data?.Price, Is.EqualTo(firstExpectedProduct.Price));

        Assert.That(result2.Data, Is.Not.Null);
        Assert.That(result2.Error, Is.Null);

        // only testing the increment here
        Assert.That(result2.Data?.Id, Is.EqualTo(result1.Data?.Id + 1));
        Assert.That(result2.Data?.Name, Is.EqualTo(secondExpectedProduct.Name));
        Assert.That(result2.Data?.Price, Is.EqualTo(secondExpectedProduct.Price));
    }

    [Test]
    public async Task GetByIdTest()
    {
        // since its a singleton I just add a product so id 1 has to exist.
        // also -1, 0 (or even 9999) should never exist and therefore should always give not found error here

        var result0 = await GetStore().CreateProduct("asd", 100);
        Assert.That(result0.Error, Is.Null);

        var result1 = await GetStore().GetProductById(1);
        Assert.That(result1.Error, Is.Null);
        Assert.That(result1.Data, Is.Not.Null);

        var result2 = await GetStore().GetProductById(-1);
        Assert.That(result2.Error, Is.Not.Null);
        Assert.That(result2.Data, Is.Null);

        var result3 = await GetStore().GetProductById(999);
        Assert.That(result3.Error, Is.Not.Null);
        Assert.That(result3.Data, Is.Null);

        var result4 = await GetStore().GetProductById(0);
        Assert.That(result4.Error, Is.Not.Null);
        Assert.That(result4.Data, Is.Null);
    }


    [Test]
    public async Task DeleteTest()
    {
        // since its a singleton I just add a product so id 1 has to exist.
        // deleting 0 should always result in error

        var result0 = await GetStore().CreateProduct("asd", 100);
        Assert.That(result0.Error, Is.Null);

        var result1 = await GetStore().GetProductById(1);
        Assert.That(result1.Error, Is.Null);
        Assert.That(result1.Data, Is.Not.Null);

        var result2 = await GetStore().DeleteProductById(result1.Data.Id);
        Assert.That(result2.Error, Is.Null);
        Assert.That(result2.Data, Is.True);

        var result3 = await GetStore().DeleteProductById(0);
        Assert.That(result3.Error, Is.Null);
        Assert.That(result3.Data, Is.True);

        var result4 = await GetStore().DeleteProductById(-1);
        Assert.That(result4.Error, Is.Null);
        Assert.That(result4.Data, Is.True);
    }

}


// I realised since I'm testing a singleton I could just use one bigger test
// to go through an entity Create, Read, Update, Delete lifecycle
// might refactor later