using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess.Abstraction
{
    public interface ITicketBookingRepository
    {
        Task<TicketBookingEntity> AddTicketBookingAsync(TicketBookingEntity ticketBooking);
        Task DeleteTicketBookingAsync(Guid ticketBookingId);
        Task<IEnumerable<TicketBookingEntity>> GetAllTicketBookingsAsync();
        Task<TicketBookingEntity> GetTicketBookingByIdAsync(Guid ticketBookingId);
        Task<TicketBookingEntity> UpdateTicketBookingAsync(TicketBookingEntity updatedTicketBooking);
    }

}
