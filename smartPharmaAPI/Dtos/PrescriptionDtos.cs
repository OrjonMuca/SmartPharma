namespace smartPharmaAPI.Dtos;

public sealed record PrescriptionRequest(
    Guid? PatientUserId,
    string PatientName,
    string DoctorName,
    string? Notes,
    DateOnly IssuedDate,
    DateOnly ExpiresDate,
    List<PrescriptionItemRequest> Items);

public sealed record PrescriptionItemRequest(Guid MedicineId, int Quantity, string? Instructions);

public sealed record UpdatePrescriptionStatusRequest(string Status);

public sealed record PrescriptionResponse(
    Guid Id,
    Guid? PatientUserId,
    string PatientName,
    string DoctorName,
    string? Notes,
    string Status,
    DateOnly IssuedDate,
    DateOnly ExpiresDate,
    DateTime CreatedAtUtc,
    List<PrescriptionItemResponse> Items);

public sealed record PrescriptionItemResponse(
    Guid Id,
    Guid MedicineId,
    string MedicineName,
    int Quantity,
    string? Instructions);
