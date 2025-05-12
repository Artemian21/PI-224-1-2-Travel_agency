﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Enums;

namespace Travel_agency.DataAccess.Entities
{
    public class HotelRoomEntity
    {
        public Guid Id { get; set; }
        public RoomType RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public Guid HotelId { get; set; }
        public HotelEntity Hotel { get; set; }
        public ICollection<HotelBookingEntity> HotelBookings { get; set; } = new List<HotelBookingEntity>();
    }
}
