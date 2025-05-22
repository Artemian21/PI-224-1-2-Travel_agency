using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly IHotelBookingService _hotelBookingService;

        public HotelBookingController(IHotelBookingService hotelBookingService)
        {
            _hotelBookingService = hotelBookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _hotelBookingService.GetAllHotelBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelBookingById(Guid id)
        {
            var booking = await _hotelBookingService.GetHotelBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotelBooking([FromBody] HotelBookingRequest hotelBooking)
        {
            if (hotelBooking == null)
            {
                return BadRequest("Hotel booking data is null.");
            }

            var hotelBookingDto = new HotelBookingDto
            {
                UserId = hotelBooking.UserId,
                HotelRoomId = hotelBooking.HotelRoomId,
                StartDate = hotelBooking.StartDate,
                EndDate = hotelBooking.EndDate,
                NumberOfGuests = hotelBooking.NumberOfGuests,
                Status = hotelBooking.Status
            };

            var createdBooking = await _hotelBookingService.AddHotelBookingAsync(hotelBookingDto);
            return Ok(createdBooking);
        }

            [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotelBooking(Guid id, [FromBody] HotelBookingRequest hotelBooking)
        {
            if (hotelBooking == null)
            {
                return BadRequest("Hotel booking data is null.");
            }

            var hotelBookingDto = new HotelBookingDto
            {
                Id = id,
                UserId = hotelBooking.UserId,
                HotelRoomId = hotelBooking.HotelRoomId,
                StartDate = hotelBooking.StartDate,
                EndDate = hotelBooking.EndDate,
                NumberOfGuests = hotelBooking.NumberOfGuests,
                Status = hotelBooking.Status
            };
            var updatedBooking = await _hotelBookingService.UpdateHotelBookingAsync(hotelBookingDto);
            if (updatedBooking == null)
            {
                return NotFound();
            }
            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelBooking(Guid id)
        {
            await _hotelBookingService.DeleteHotelBookingAsync(id);
            return NoContent();
        }
    }
}
