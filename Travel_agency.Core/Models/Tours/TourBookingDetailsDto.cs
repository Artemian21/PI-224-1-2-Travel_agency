using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models.Users;

namespace Travel_agency.Core.Models.Tours
{
    public class TourBookingDetailsDto : TourBookingDto
    {
        public TourDto Tour { get; set; }
        public UserDto User { get; set; }
    }
}
