using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.Models.Transports
{
    public class TransportWithBookingsDto : TransportDto
    {
        public ICollection<TicketBookingDto> TicketBookings { get; set; } = new List<TicketBookingDto>();
    }
}
