using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models
{
    public record HotelBookingRequest(
        Guid HotelRoomId,
        Guid UserId,
        DateTime StartDate,
        DateTime EndDate,
        int NumberOfGuests,
        Status Status);
}
