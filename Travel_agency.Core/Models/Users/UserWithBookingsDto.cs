using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.Core.Models.Tours;
using Travel_agency.Core.Models.Transports;

namespace Travel_agency.Core.Models.Users
{
    public class UserWithBookingsDto : UserDto
    {
        public ICollection<HotelBookingDto> HotelBookings { get; set; } = new List<HotelBookingDto>();
        public ICollection<TicketBookingDto> TicketBookings { get; set; } = new List<TicketBookingDto>();
        public ICollection<TourBookingDto> TourBookings { get; set; } = new List<TourBookingDto>();
    }
}