namespace MedicalScanBackend.Repository.Store;

public static class ProductErrors
{
    public static string CouldNotFind { get; set; } = "product not found";
    public static string CouldNotCreate { get; set; } = "could not create product";
    public static string CouldNotUpdate { get; set; } = "could not update product";
    public static string CouldNotDelete { get; set; } = "could not delete product";

}