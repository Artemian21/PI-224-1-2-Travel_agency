using Travel_agency.Core.Enums;

namespace Travel_agency.Core.Models.Users;

public class UserDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Registered;
}