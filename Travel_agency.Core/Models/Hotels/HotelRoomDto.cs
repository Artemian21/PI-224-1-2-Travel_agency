using Travel_agency.Core.Enums;

namespace Travel_agency.Core.Models.Hotels;

public class HotelRoomDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RoomType RoomType { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid HotelId { get; set; }
}