using Travel_agency.Core.Enums;

namespace Travel_agency.Core.Models;

public class HotelBookingDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HotelRoomId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public HotelRoomDto HotelRoom { get; set; }
    public UserDto User { get; set; }
}