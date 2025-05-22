using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.Models.Hotels
{
    public class HotelRoomWithBookingDto : HotelRoomDto
    {
        public HotelDto Hotel { get; set; }
        public ICollection<HotelBookingDto> HotelBookings { get; set; } = new List<HotelBookingDto>();
    }
}
