namespace smartPharmaAPI.Dtos;

public sealed record SupplierRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address);

public sealed record SupplierResponse(
    Guid Id,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address,
    DateTime CreatedAtUtc);
