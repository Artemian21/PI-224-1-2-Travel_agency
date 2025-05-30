using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.Models.Tours
{
    public class TourWithBookingsDto : TourDto
    {
        public ICollection<TourBookingDto> TourBookings { get; set; } = new List<TourBookingDto>();
    }
}
