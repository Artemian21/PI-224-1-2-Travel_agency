using System.ComponentModel.DataAnnotations;

namespace Travel_agency.PL.Models.Requests
{
    public record LoginRequest(
        [EmailAddress] string Email,
        string Password
        );
}
