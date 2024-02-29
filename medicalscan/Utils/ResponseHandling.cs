using medicalscan.Core;

namespace medicalscan.Utils;

public static class ResponseHandling
{
    public static HandledResponse<T> MakeStatusCodeResponse<T>(int code, T? data, string? error)
    {
        return new HandledResponse<T>
        {
            Data = data,
            Code = 200,
            Error = error
        };
    }
    
    
    public static HandledResponse<T> MakeOkResponse<T>(T data)
    {
        return MakeStatusCodeResponse(200, data, null);
    }
}