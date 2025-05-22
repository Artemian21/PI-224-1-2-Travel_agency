using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Tours;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourBookingController : ControllerBase
    {
        private readonly ITourBookingService _tourBookingService;

        public TourBookingController(ITourBookingService tourBookingService)
        {
            _tourBookingService = tourBookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _tourBookingService.GetAllTourBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourBookingById(Guid id)
        {
            var booking = await _tourBookingService.GetTourBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTourBooking([FromBody] TourBookingRequest tourBooking)
        {
            if (tourBooking == null)
            {
                return BadRequest("Tour booking data is null.");
            }

            var tourBookingDto = new TourBookingDto
            {
                UserId = tourBooking.UserId,
                TourId = tourBooking.TourId,
                Status = tourBooking.Status
            };

            var createdBooking = await _tourBookingService.AddTourBookingAsync(tourBookingDto);
            return Ok(createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTourBooking(Guid id, [FromBody] TourBookingRequest tourBooking)
        {
            if (tourBooking == null)
            {
                return BadRequest("Tour booking data is null.");
            }

            var tourBookingDto = new TourBookingDto
            {
                Id = id,
                UserId = tourBooking.UserId,
                TourId = tourBooking.TourId,
                Status = tourBooking.Status
            };

            var updatedBooking = await _tourBookingService.UpdateTourBookingAsync(tourBookingDto);
            if (updatedBooking == null)
            {
                return NotFound();
            }
            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourBooking(Guid id)
        {
            var deleted = await _tourBookingService.DeleteTourBookingAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
