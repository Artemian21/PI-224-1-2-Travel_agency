using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models
{
    public record TourBookingRequest(
        Guid TourId,
        Guid UserId,
        Status Status);
}
