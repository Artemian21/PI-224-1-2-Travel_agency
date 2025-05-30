using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models.Hotels;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelService
    {
        Task<HotelDto> AddHotelAsync(HotelDto hotelDto);
        Task<bool> DeleteHotelAsync(Guid hotelId);
        Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
        Task<HotelWithBookingsDto?> GetHotelByIdAsync(Guid hotelId);
        Task<HotelDto?> UpdateHotelAsync(HotelDto hotelDto);
    }
}
