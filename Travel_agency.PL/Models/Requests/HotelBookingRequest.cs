using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record HotelBookingRequest(
        Guid HotelRoomId,
        DateTime StartDate,
        DateTime EndDate,
        int NumberOfGuests,
        Status Status);
}
