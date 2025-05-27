using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Responses
{
    public record TicketBookingResponse(
        Guid Id,
        Guid TransportId,
        Guid UserId,
        DateTime BookingDate,
        Status Status);
}
