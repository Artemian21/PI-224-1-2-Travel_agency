using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.DataAccess.Entities
{
    public class TourEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<TourBookingEntity> TourBookings { get; set; } = new List<TourBookingEntity>();

    }
}
