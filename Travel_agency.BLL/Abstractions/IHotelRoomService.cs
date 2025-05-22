using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models.Hotels;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelRoomService
    {
        Task<HotelRoomDto> AddHotelRoomAsync(HotelRoomDto hotelRoomDto);
        Task<bool> DeleteHotelRoomAsync(Guid hotelRoomId);
        Task<IEnumerable<HotelRoomDto>> GetAllHotelRoomsAsync();
        Task<HotelRoomWithBookingDto?> GetHotelRoomByIdAsync(Guid hotelRoomId);
        Task<IEnumerable<HotelRoomDto>> GetHotelRoomsByHotelIdAsync(Guid hotelId);
        Task<HotelRoomDto?> UpdateHotelRoomAsync(HotelRoomDto hotelRoomDto);
    }
}
