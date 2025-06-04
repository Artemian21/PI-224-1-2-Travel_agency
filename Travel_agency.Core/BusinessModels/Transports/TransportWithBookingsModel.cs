using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.BusinessModels.Transports
{
    public class TransportWithBookingsModel : TransportModel
    {
        public ICollection<TicketBookingModel> TicketBookings { get; set; } = new List<TicketBookingModel>();
    }
}
