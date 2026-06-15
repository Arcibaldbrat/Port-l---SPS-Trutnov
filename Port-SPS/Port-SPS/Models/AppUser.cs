using System.ComponentModel.DataAnnotations;

namespace Port_SPS.Models;

public class AppUser
{
    public int Id { get; set; }

    [MaxLength(80)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(32)]
    public string Role { get; set; } = UserRoles.Student;

    [MaxLength(40)]
    public string? ClassName { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
