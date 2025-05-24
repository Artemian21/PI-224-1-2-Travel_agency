using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models
{
    public record TicketBookingRequest(
         Guid TransportId,
         Status Status);
}
