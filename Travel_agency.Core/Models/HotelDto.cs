namespace Travel_agency.Core.Models;

public class HotelDto
{
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<HotelRoomDto> HotelRooms { get; set; } = new List<HotelRoomDto>();
}