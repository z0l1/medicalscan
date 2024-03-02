using System.ComponentModel.DataAnnotations;
using MedicalScanBackend.Core.Errors;
using Newtonsoft.Json;

namespace MedicalScanBackend.Core.DTOs;

public class ProductDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [MinLength(3, ErrorMessage = ProductValidationErrors.NameTooShort)]
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = ProductValidationErrors.PriceNotPositive)]
    [JsonProperty("price")]
    public decimal Price { get; set; }
}