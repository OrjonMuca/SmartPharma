namespace smartPharmaAPI.Dtos;

public sealed record InventoryItemRequest(
    Guid MedicineId,
    string BatchNumber,
    int Quantity,
    int ReorderLevel,
    DateOnly ExpirationDate,
    Guid? SupplierId);

public sealed record UpdateInventoryQuantityRequest(int Quantity);

public sealed record InventoryItemResponse(
    Guid Id,
    Guid MedicineId,
    string MedicineName,
    string BatchNumber,
    int Quantity,
    int ReorderLevel,
    bool LowStock,
    DateOnly ExpirationDate,
    Guid? SupplierId,
    string? SupplierName,
    DateTime LastUpdatedAtUtc);
