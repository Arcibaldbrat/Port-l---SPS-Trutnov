namespace Port_SPS.Models;

public record LoginRequest(string Username, string Password, bool RememberMe);

public record RegisterRequest(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string Role,
    string? ClassName);

public record UserResponse(
    int Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string? ClassName);
