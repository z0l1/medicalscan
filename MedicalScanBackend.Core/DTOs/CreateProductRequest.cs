using Newtonsoft.Json;

namespace MedicalScanBackend.Core.DTOs;

public class CreateProductRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("price")]
    public decimal Price { get; set; }
}