using System.ComponentModel.DataAnnotations;

namespace Travel_agency.PL.Models
{
    public record LoginRequest(
        [EmailAddress] string Email,
        string Password
        );
}
