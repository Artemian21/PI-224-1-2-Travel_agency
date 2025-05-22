using System.ComponentModel.DataAnnotations;
using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models
{
    public record UserResponse(
        Guid Id,
        string Username,
        string Email,
        UserRole Role);
}
