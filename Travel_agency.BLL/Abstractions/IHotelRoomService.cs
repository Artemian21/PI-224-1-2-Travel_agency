using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions
{
    public interface IHotelRoomService
    {
        Task<HotelRoomDto> AddHotelRoomAsync(HotelRoomDto hotelRoomDto);
        Task<bool> DeleteHotelRoomAsync(Guid hotelRoomId);
        Task<IEnumerable<HotelRoomDto>> GetAllHotelRoomsAsync();
        Task<HotelRoomDto?> GetHotelRoomByIdAsync(Guid hotelRoomId);
        Task<HotelRoomDto?> UpdateHotelRoomAsync(HotelRoomDto hotelRoomDto);
    }
}
