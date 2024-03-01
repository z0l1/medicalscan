namespace MedicalScanBackend.Core.DTOs;

public class HandledResponse<T>
{
    public int Code { get; set; }
    public string? Error { get; set; }
    public T? Data { get; set; }
}