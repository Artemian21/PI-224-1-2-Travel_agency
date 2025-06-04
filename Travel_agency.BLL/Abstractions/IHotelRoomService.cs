using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.BusinessModels.Hotels;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelRoomService
    {
        Task<HotelRoomModel> AddHotelRoomAsync(HotelRoomModel hotelRoomModel);
        Task<bool> DeleteHotelRoomAsync(Guid hotelRoomId);
        Task<IEnumerable<HotelRoomModel>> GetAllHotelRoomsAsync();
        Task<HotelRoomWithBookingModel?> GetHotelRoomByIdAsync(Guid hotelRoomId);
        Task<IEnumerable<HotelRoomModel>> GetHotelRoomsByHotelIdAsync(Guid hotelId);
        Task<HotelRoomModel?> UpdateHotelRoomAsync(HotelRoomModel hotelRoomModel);
    }
}
