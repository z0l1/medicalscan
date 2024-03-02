namespace MedicalScanBackend.DomainLogic.Errors;

public static class ProductValidationErrors
{
    public static string PriceNotPositive => "Product price must be a positive number.";
    public static string NameTooShort => "Product name must be at least 3 characters.";
}