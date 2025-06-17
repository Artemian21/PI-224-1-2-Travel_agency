using Travel_agency.Core.Enums;

namespace Travel_agency.Core.BusinessModels.Hotels;

public class HotelRoomModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RoomType RoomType { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid HotelId { get; set; }
}