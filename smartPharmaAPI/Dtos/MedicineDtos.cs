namespace smartPharmaAPI.Dtos;

public sealed record MedicineRequest(
    string Name,
    string? GenericName,
    string? Description,
    string Manufacturer,
    string DosageForm,
    string Strength,
    string Category,
    decimal Price,
    bool RequiresPrescription,
    bool IsActive = true);

public sealed record MedicineResponse(
    Guid Id,
    string Name,
    string? GenericName,
    string? Description,
    string Manufacturer,
    string DosageForm,
    string Strength,
    string Category,
    decimal Price,
    bool RequiresPrescription,
    bool IsActive,
    int StockQuantity,
    bool LowStock);
