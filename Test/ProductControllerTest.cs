using System.ComponentModel.DataAnnotations;
using MedicalScanBackend.Controllers;
using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Errors;
using MedicalScanBackend.DomainLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
                    // any response we return a 200 mock value, since we validate at controller level
                    It.Is<CreateProductRequest>(r => true)
                ))
            .ReturnsAsync(new HandledResponse<ProductDto?>
            {
                Code = 200,
                Data = new ProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "mock",
                    Price = 123
                }
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
        
        ValidateModel(req, _productController);

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

        ValidateModel(req, _productController);

        var result = await _productController.CreateProduct(req);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Error, Is.Not.Null);
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

        ValidateModel(req, _productController);

        var result = await _productController.CreateProduct(req);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Code, Is.EqualTo(400));
        Assert.That(result.Data, Is.Null);
    }


    private static void ValidateModel(object model, ControllerBase controller)
    {
        controller.ModelState.Clear();
        
        var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(model, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage);
            }
        }
    }
}