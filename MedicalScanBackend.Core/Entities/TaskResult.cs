namespace medicalscan.Core.Entities;

public class TaskResult<T>
{
    public TaskResult(T? data = default, string? error = null, string? errorPrefix = null)
    {
        Data = data;
        Error = errorPrefix == null ? error : $"{errorPrefix}: {error}";
    }
        
    public T? Data { get; set; }
    public string? Error { get; set; }
}