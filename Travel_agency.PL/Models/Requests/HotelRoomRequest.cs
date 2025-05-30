using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record HotelRoomRequest(
        RoomType RoomType,
        int Capacity,
        decimal PricePerNight,
        Guid HotelId);
}
