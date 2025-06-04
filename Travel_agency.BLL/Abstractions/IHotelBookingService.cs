﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.BusinessModels.Hotels;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelBookingService
    {
        Task<HotelBookingModel> AddHotelBookingAsync(HotelBookingModel hotelBookingModel);
        Task<bool> DeleteHotelBookingAsync(Guid hotelBookingId);
        Task<IEnumerable<HotelBookingModel>> GetAllHotelBookingsAsync();
        Task<HotelBookingDetailsModel> GetHotelBookingByIdAsync(Guid hotelBookingId);
        Task<HotelBookingModel> UpdateHotelBookingAsync(HotelBookingModel hotelBookingModel);
    }
}
