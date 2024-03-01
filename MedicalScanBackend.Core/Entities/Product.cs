namespace MedicalScanBackend.Core.Entities;

public class Product
{
    // to hide the database ID the product
    public Guid Uuid { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}