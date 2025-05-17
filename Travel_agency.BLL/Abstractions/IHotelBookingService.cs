using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelBookingService
    {
        Task<HotelBookingDto> AddHotelBookingAsync(HotelBookingDto hotelBookingDto);
        Task<bool> DeleteHotelBookingAsync(Guid hotelBookingId);
        Task<IEnumerable<HotelBookingDto>> GetAllHotelBookingsAsync();
        Task<HotelBookingDto> GetHotelBookingByIdAsync(Guid hotelBookingId);
        Task<HotelBookingDto> UpdateHotelBookingAsync(HotelBookingDto hotelBookingDto);
    }
}
