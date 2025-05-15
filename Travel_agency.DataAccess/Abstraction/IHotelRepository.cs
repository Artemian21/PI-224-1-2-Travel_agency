using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess.Abstraction
{
    public interface IHotelRepository
    {
        Task<HotelEntity> AddHotelAsync(HotelEntity hotel);
        Task<IEnumerable<HotelEntity>> GetAllHotelsAsync();
        Task<HotelEntity> GetHotelByIdAsync(Guid hotelId);
        Task<HotelEntity> UpdateHotelAsync(HotelEntity updatedHotel);
        Task DeleteHotelAsync(Guid hotelId);
    }

}
