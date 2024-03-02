using MedicalScanBackend.Controllers;
using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.DomainLogic.Errors;
using MedicalScanBackend.DomainLogic.Services.Interfaces;
using Moq;

namespace Test;

public class ProductControllerTest
{
    private Mock<IProductService> _mockProductService;
    private ProductController _productController;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockProductService.Setup(
                service => service.CreateProductAsync(
                    It.Is<CreateProductRequest>(r => r.Name == "abc" && r.Price == 123)
                ))
            .ReturnsAsync(new HandledResponse<ProductDto?>
            {
                Code = 200,
                Data = new ProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "abc",
                    Price = 123
                }
            });

        _mockProductService.Setup(
                service => service.CreateProductAsync(
                    It.Is<CreateProductRequest>(r => r.Name == "a" && r.Price == 123)
                ))
            .ReturnsAsync(new HandledResponse<ProductDto?>
            {
                Code = 400,
                Data = null,
                Error = RequestErrors.NameLength
            });

        _mockProductService.Setup(
                service => service.CreateProductAsync(
                    It.Is<CreateProductRequest>(r => r.Name == "abc" && r.Price == 0)
                ))
            .ReturnsAsync(new HandledResponse<ProductDto?>
            {
                Code = 400,
                Data = null,
                Error = RequestErrors.PriceNotPositive
            });

        _productController = new ProductController(_mockProductService.Object);
    }


    [Test]
    public void ControllerInstantiatedTest()
    {
        Assert.That(_productController, Is.Not.Null);
    }

    [Test]
    public async Task ControllerCreateTest()
    {
        var req = new CreateProductRequest
        {
            Name = "abc",
            Price = 123
        };

        var result = await _productController.CreateProduct(req);

        Assert.That(_productController, Is.Not.Null);
        // Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Null);

        Assert.That(result.Error, Is.Null);
        Assert.That(result.Code, Is.EqualTo(200));
        Assert.That(result.Data, Is.Not.Null);
    }
    
    [Test]
    public async Task ControllerCreateShortNameTest()
    {
        var req = new CreateProductRequest
        {
            Name = "a",
            Price = 123
        };

        var result = await _productController.CreateProduct(req);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error, Is.EqualTo(RequestErrors.NameLength));
        Assert.That(result.Code, Is.EqualTo(400));
        Assert.That(result.Data, Is.Null);
    }
    
    [Test]
    public async Task ControllerCreateWrongPriceTest()
    {
        var req = new CreateProductRequest
        {
            Name = "abc",
            Price = 0
        };

        var result = await _productController.CreateProduct(req);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error, Is.EqualTo(RequestErrors.PriceNotPositive));
        Assert.That(result.Code, Is.EqualTo(400));
        Assert.That(result.Data, Is.Null);
    }
}