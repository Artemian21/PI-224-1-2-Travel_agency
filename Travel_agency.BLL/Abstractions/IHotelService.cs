using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.BusinessModels.Hotels;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelService
    {
        Task<HotelModel> AddHotelAsync(HotelModel hotelModel);
        Task<bool> DeleteHotelAsync(Guid hotelId);
        Task<IEnumerable<HotelModel>> GetAllHotelsAsync();
        Task<HotelWithBookingsModel?> GetHotelByIdAsync(Guid hotelId);
        Task<HotelModel?> UpdateHotelAsync(HotelModel hotelModel);
    }
}
