using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.PL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IHotelRoomService _hotelRoomService;

        public HotelRoomController(IHotelRoomService hotelRoomService)
        {
            _hotelRoomService = hotelRoomService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHotelRooms()
        {
            var hotelRooms = await _hotelRoomService.GetAllHotelRoomsAsync();
            return Ok(hotelRooms);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelRoomById(Guid id)
        {
            var hotelRoom = await _hotelRoomService.GetHotelRoomByIdAsync(id);
            if (hotelRoom == null)
            {
                return NotFound();
            }
            return Ok(hotelRoom);
        }

        [HttpGet("hotel/{hotelId}/rooms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelRoomsByHotelId(Guid hotelId)
        {
            var rooms = await _hotelRoomService.GetHotelRoomsByHotelIdAsync(hotelId);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> CreateHotelRoom([FromBody] HotelRoomRequest hotelRoom)
        {
            if (hotelRoom == null)
            {
                return BadRequest("Invalid hotel room data");
            }

            var hotelRoomDto = new HotelRoomDto
            {
                RoomType = hotelRoom.RoomType,
                Capacity = hotelRoom.Capacity,
                PricePerNight = hotelRoom.PricePerNight,
                HotelId = hotelRoom.HotelId
            };

            var createdHotelRoom = await _hotelRoomService.AddHotelRoomAsync(hotelRoomDto);
            return Ok(createdHotelRoom);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> UpdateHotelRoom(Guid id, [FromBody] HotelRoomRequest hotelRoom)
        {
            if (hotelRoom == null)
            {
                return BadRequest("Invalid hotel room data");
            }

            var hotelRoomDto = new HotelRoomDto
            {
                Id = id,
                RoomType = hotelRoom.RoomType,
                Capacity = hotelRoom.Capacity,
                PricePerNight = hotelRoom.PricePerNight,
                HotelId = hotelRoom.HotelId
            };

            var updatedHotelRoom = await _hotelRoomService.UpdateHotelRoomAsync(hotelRoomDto);
            if (updatedHotelRoom == null)
            {
                return NotFound();
            }
            return Ok(updatedHotelRoom);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> DeleteHotelRoom(Guid id)
        {
            var hotelRoom = await _hotelRoomService.GetHotelRoomByIdAsync(id);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            await _hotelRoomService.DeleteHotelRoomAsync(id);
            return NoContent();
        }
    }
}
