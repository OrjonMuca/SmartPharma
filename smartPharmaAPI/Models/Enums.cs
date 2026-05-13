namespace smartPharmaAPI.Models;

public enum UserRole
{
    Admin,
    Pharmacist,
    Customer
}

public enum PrescriptionStatus
{
    Pending,
    Approved,
    Rejected,
    Fulfilled
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Dispensed,
    Cancelled
}
