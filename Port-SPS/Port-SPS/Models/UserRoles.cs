namespace Port_SPS.Models;

public static class UserRoles
{
    public const string Student = "Student";
    public const string Teacher = "Teacher";
    public const string Admin = "Admin";

    public static bool IsValid(string? role)
    {
        return role is Student or Teacher or Admin;
    }
}
