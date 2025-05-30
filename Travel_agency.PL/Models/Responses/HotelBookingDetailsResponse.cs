using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Responses
{
    public record HotelBookingDetailsResponse(
        Guid Id,
        Guid HotelRoomId,
        Guid UserId,
        DateTime StartDate,
        DateTime EndDate,
        int NumberOfGuests,
        Status Status,
        HotelRoomResponse HotelRoom,
        UserResponse User);
}
