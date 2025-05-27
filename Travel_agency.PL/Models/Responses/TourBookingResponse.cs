using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Responses
{
    public record TourBookingResponse(
        Guid Id,
        Guid TourId,
        Guid UserId,
        DateTime BookingDate,
        Status Status);
}
