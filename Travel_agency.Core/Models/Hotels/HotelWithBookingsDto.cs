using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.Models.Hotels
{
    public class HotelWithBookingsDto : HotelDto
    {
        public List<HotelRoomDto> HotelRooms { get; set; } = new List<HotelRoomDto>();
    }
}
