using MedicalScan.Core.Entities;
using MedicalScan.Repository.Store;

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

    [Test]
    public async Task StoreTest()
    {
        // check 0 count
        var getAllResult = await GetStore().GetAllProducts();
        Assert.That(getAllResult.Error, Is.Null);
        Assert.That(getAllResult.Data?.Count(), Is.EqualTo(0));

        // add one
        var createResult = await GetStore().CreateProduct("first", 100);
        Assert.That(createResult.Error, Is.Null);

        // check 1 count
        getAllResult = await GetStore().GetAllProducts();
        Assert.That(getAllResult.Error, Is.Null);
        Assert.That(getAllResult.Data?.Count(), Is.EqualTo(1));

        // get by created id
        var getByIdResult = await GetStore().GetProductById(createResult.Data.Id);
        Assert.That(getByIdResult.Error, Is.Null);
        Assert.That(getByIdResult.Data, Is.Not.Null);

        Assert.That(getByIdResult.Data?.Id, Is.EqualTo(createResult.Data?.Id));
        Assert.That(getByIdResult.Data?.Name, Is.EqualTo("first"));
        Assert.That(getByIdResult.Data?.Price, Is.EqualTo(100));

        // add second to check id increment
        var createResult1 = await GetStore().CreateProduct("second", 200);
        Assert.That(createResult1.Error, Is.Null);
        Assert.That(createResult1.Data, Is.Not.Null);
        Assert.That(createResult1.Data?.Id, Is.EqualTo(createResult.Data?.Id + 1));

        // check update
        var updated = createResult.Data;
        updated.Name = "firstUpdated";
        updated.Price = 12345;
        var updateResult = await GetStore().UpdateProduct(updated);
        Assert.That(updateResult.Error, Is.Null);
        Assert.That(updateResult.Data, Is.Not.Null);
        Assert.That(updateResult.Data?.Name, Is.EqualTo(updated.Name));
        Assert.That(updateResult.Data?.Price, Is.EqualTo(updated.Price));

        // check delete all
        var deleteResult = await GetStore().DeleteProductById(createResult.Data.Id);
        Assert.That(deleteResult.Error, Is.Null);
        Assert.That(deleteResult.Data, Is.True);

        deleteResult = await GetStore().DeleteProductById(createResult1.Data.Id);
        Assert.That(deleteResult.Error, Is.Null);
        Assert.That(deleteResult.Data, Is.True);

        // check after delete
        getAllResult = await GetStore().GetAllProducts();
        Assert.That(getAllResult.Error, Is.Null);
        Assert.That(getAllResult.Data?.Count(), Is.EqualTo(0));
    }


    // check get nonexistent, update nonexistent, delete nonexistent
    [Test]
    public async Task TestOutliers()
    {
        var getResult = await GetStore().GetProductById(0);
        Assert.That(getResult.Error, Is.Not.Null);
        Assert.That(getResult.Error, Is.EqualTo(ProductErrors.CouldNotFind));
        Assert.That(getResult.Data, Is.Null);

        var updateResult = await GetStore()
            .UpdateProduct(new Product { Id = 0, Price = 1, Name = "asd" });
        Assert.That(updateResult.Error, Is.Not.Null);
        Assert.That(updateResult.Error, Is.EqualTo(ProductErrors.CouldNotFind));
        Assert.That(updateResult.Data, Is.Null);
        
        var deleteResult = await GetStore().DeleteProductById(0);
        Assert.That(deleteResult.Error, Is.Not.Null);
        Assert.That(deleteResult.Error, Is.EqualTo(ProductErrors.CouldNotFind));
        Assert.That(deleteResult.Data, Is.False);
    }
}
