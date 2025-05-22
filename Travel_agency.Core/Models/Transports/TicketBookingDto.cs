using Travel_agency.Core.Enums;

namespace Travel_agency.Core.Models.Transports;

public class TicketBookingDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TransportId { get; set; }
    public Guid UserId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Pending;
}