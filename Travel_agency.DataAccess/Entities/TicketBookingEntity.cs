using Travel_agency.Core.Enums;

namespace Travel_agency.DataAccess.Entities
{
    public class TicketBookingEntity
    {
        public Guid Id { get; set; }
        public Guid TransportId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public Status Status { get; set; }
        public TransportEntity Transport { get; set; }
        public UserEntity User { get; set; }
    }
}