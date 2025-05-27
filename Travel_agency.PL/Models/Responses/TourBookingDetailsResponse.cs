using Travel_agency.Core.Enums;
using Travel_agency.Core.Models.Tours;
using Travel_agency.Core.Models.Users;

namespace Travel_agency.PL.Models.Responses
{
    public record TourBookingDetailsResponse(
        Guid Id,
        Guid TourId,
        Guid UserId,
        DateTime BookingDate,
        Status Status,
        TourDto Tour,
        UserDto User);
}
