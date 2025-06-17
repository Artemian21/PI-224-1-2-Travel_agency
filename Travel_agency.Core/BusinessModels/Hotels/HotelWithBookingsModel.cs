using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.BusinessModels.Hotels
{
    public class HotelWithBookingsModel : HotelModel
    {
        public List<HotelRoomModel> HotelRooms { get; set; } = new List<HotelRoomModel>();
    }
}
