using MedicalScanBackend.Core.DTOs;
using MedicalScanBackend.Core.Entities;

namespace MedicalScanBackend.Core.Mappers;

public static class ProductMapper
{
    public static ProductDto EntityToDto(Product entity)
    {
        return new ProductDto
        {
            Id = entity.Uuid,
            Name = entity.Name,
            Price = entity.Price
        };
    }
}