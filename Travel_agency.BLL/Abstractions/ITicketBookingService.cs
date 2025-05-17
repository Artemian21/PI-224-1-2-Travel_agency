using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITicketBookingService
    {
        Task<TicketBookingDto> AddTicketBookingAsync(TicketBookingDto ticketBookingDto);
        Task<bool> DeleteTicketBookingAsync(Guid ticketBookingId);
        Task<IEnumerable<TicketBookingDto>> GetAllTicketBookingsAsync();
        Task<TicketBookingDto?> GetTicketBookingByIdAsync(Guid ticketBookingId);
        Task<TicketBookingDto?> UpdateTicketBookingAsync(TicketBookingDto ticketBookingDto);
    }
}