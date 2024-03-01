using Newtonsoft.Json;

namespace MedicalScanBackend.Core.DTOs;

public class ProductDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("price")]
    public decimal Price { get; set; }
}