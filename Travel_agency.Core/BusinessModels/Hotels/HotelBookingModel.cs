using Travel_agency.Core.Enums;

namespace Travel_agency.Core.BusinessModels.Hotels;

public class HotelBookingModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HotelRoomId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public Status Status { get; set; } = Status.Pending;
}