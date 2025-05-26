using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Responses
{
    public record HotelRoomResponse(
        Guid Id,
        RoomType RoomType,
        int Capacity,
        decimal PricePerNight,
        Guid HotelId);
}
