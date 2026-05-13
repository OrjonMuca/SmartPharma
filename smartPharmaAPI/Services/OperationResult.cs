namespace smartPharmaAPI.Services;

public sealed record OperationResult<T>(bool Succeeded, T? Value, string? Error)
{
    public static OperationResult<T> Success(T value) => new(true, value, null);

    public static OperationResult<T> Failure(string error) => new(false, default, error);
}
