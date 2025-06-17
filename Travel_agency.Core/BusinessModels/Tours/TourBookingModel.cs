using Travel_agency.Core.Enums;

namespace Travel_agency.Core.BusinessModels.Tours;

public class TourBookingModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TourId { get; set; }
    public Guid UserId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Pending;
}