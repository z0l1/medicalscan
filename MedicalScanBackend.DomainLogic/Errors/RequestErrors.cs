namespace MedicalScanBackend.DomainLogic.Errors;

public static class RequestErrors
{
    public static string PriceNotPositive => "Product price must be a positive number!";
    public static string NameLength => "Product name must be at least 3 characters";
}