using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record TourBookingRequest(
        Guid TourId,
        Status Status);
}
