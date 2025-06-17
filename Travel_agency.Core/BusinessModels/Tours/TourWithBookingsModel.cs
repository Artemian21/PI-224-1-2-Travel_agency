using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.BusinessModels.Tours
{
    public class TourWithBookingsModel : TourModel
    {
        public ICollection<TourBookingModel> TourBookings { get; set; } = new List<TourBookingModel>();
    }
}
