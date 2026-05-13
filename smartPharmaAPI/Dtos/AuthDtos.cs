namespace smartPharmaAPI.Dtos;

public sealed record RegisterRequest(string FullName, string Email, string Password);

public sealed record LoginRequest(string Email, string Password);

public sealed record CreateUserRequest(string FullName, string Email, string Password, string Role);

public sealed record UpdateUserRoleRequest(string Role);

public sealed record AuthResponse(string Token, DateTime ExpiresAtUtc, UserResponse User);

public sealed record UserResponse(Guid Id, string FullName, string Email, string Role);
