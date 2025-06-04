using Travel_agency.Core.BusinessModels.Transports;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITicketBookingService
    {
        Task<TicketBookingModel> AddTicketBookingAsync(TicketBookingModel ticketBookingModel);
        Task<bool> DeleteTicketBookingAsync(Guid ticketBookingId);
        Task<IEnumerable<TicketBookingModel>> GetAllTicketBookingsAsync();
        Task<TicketBookingDetailsModel?> GetTicketBookingByIdAsync(Guid ticketBookingId);
        Task<TicketBookingModel?> UpdateTicketBookingAsync(TicketBookingModel ticketBookingModel);
    }
}