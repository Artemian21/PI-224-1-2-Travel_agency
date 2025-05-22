using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models.Users;

namespace Travel_agency.Core.Models.Transports
{
    public class TicketBookingDetailsDto : TicketBookingDto
    {
        public TransportDto Transport { get; set; }
        public UserDto User { get; set; }
    }
}
