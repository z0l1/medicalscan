namespace MedicalScanBackend.Core.Errors;

public static class ProductValidationErrors
{
    public const string PriceNotPositive = "Product price must be a positive number.";
    public const string NameTooShort = "Product name must be at least 3 characters.";
}