using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models
{
    public record HotelRoomRequest(
        RoomType RoomType, 
        int Capacity, 
        decimal PricePerNight, 
        Guid HotelId);
}
