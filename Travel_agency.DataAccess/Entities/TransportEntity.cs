using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.DataAccess.Entities
{
    public class TransportEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal Price { get; set; }
        public ICollection<TicketBookingEntity> TicketBookings { get; set; } = new List<TicketBookingEntity>();
    }
}
