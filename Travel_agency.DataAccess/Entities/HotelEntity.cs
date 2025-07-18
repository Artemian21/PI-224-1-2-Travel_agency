﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.DataAccess.Entities
{
    public class HotelEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<HotelRoomEntity> HotelRoom { get; set; } = new List<HotelRoomEntity>();
    }
}
