using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.BusinessModels.Hotels
{
    public class HotelRoomWithBookingModel : HotelRoomModel
    {
        public HotelModel Hotel { get; set; }
        public ICollection<HotelBookingModel> HotelBookings { get; set; } = new List<HotelBookingModel>();
    }
}
