using System.ComponentModel.DataAnnotations;

namespace Travel_agency.PL.Models.Requests
{
    public record RegisterUserRequest(
        string Username,
        [EmailAddress] string Email,
        string Password);
}
