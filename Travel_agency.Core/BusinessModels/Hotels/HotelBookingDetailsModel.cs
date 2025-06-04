using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.BusinessModels.Users;

namespace Travel_agency.Core.BusinessModels.Hotels
{
    public class HotelBookingDetailsModel : HotelBookingModel
    {
        public HotelRoomModel HotelRoom { get; set; }
        public UserModel User { get; set; }
    }
}
