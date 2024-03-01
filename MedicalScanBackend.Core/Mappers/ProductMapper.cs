using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.Core.Mappers;

public static class ProductMapper
{
    public static ProductDto EntityToDto(Product entity, Guid key)
    {
        return new ProductDto
        {
            Id = key,
            Name = entity.Name,
            Price = entity.Price
        };
    }
}