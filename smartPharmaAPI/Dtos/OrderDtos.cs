namespace smartPharmaAPI.Dtos;

public sealed record OrderRequest(
    Guid? CustomerUserId,
    string CustomerName,
    Guid? PrescriptionId,
    List<OrderItemRequest> Items);

public sealed record OrderItemRequest(Guid MedicineId, int Quantity);

public sealed record UpdateOrderStatusRequest(string Status);

public sealed record OrderResponse(
    Guid Id,
    Guid? CustomerUserId,
    string CustomerName,
    Guid? PrescriptionId,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAtUtc,
    List<OrderItemResponse> Items);

public sealed record OrderItemResponse(
    Guid Id,
    Guid MedicineId,
    string MedicineName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
