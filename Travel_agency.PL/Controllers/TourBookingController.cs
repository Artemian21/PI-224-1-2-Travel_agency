using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            var allBookings = await _tourBookingService.GetAllTourBookingsAsync();

            if (User.IsInRole("Admin"))
            {
                return Ok(allBookings);
            }
            else
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value);
                var userBookings = allBookings.Where(b => b.UserId == userId).ToList();
                return Ok(userBookings);
            }
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

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var tourBookingDto = new TourBookingDto
            {
                UserId = userId,
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

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var tourBookingDto = new TourBookingDto
            {
                Id = id,
                UserId = userId,
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
