using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Tours;
using Travel_agency.PL.Models.Requests;
using Travel_agency.PL.Models.Responses;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TourBookingController : ControllerBase
    {
        private readonly ITourBookingService _tourBookingService;
        private readonly IMapper _mapper;

        public TourBookingController(ITourBookingService tourBookingService, IMapper mapper)
        {
            _tourBookingService = tourBookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var allBookings = await _tourBookingService.GetAllTourBookingsAsync();

            if (User.IsInRole("Admin"))
            {
                return Ok(_mapper.Map<List<TourBookingResponse>>(allBookings));
            }
            else
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value);
                var userBookings = allBookings.Where(b => b.UserId == userId).ToList();
                return Ok(_mapper.Map<List<TourBookingResponse>>(userBookings));
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
            return Ok(_mapper.Map<TourBookingDetailsResponse>(booking));
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

            var tourBookingDto = _mapper.Map<TourBookingDto>(tourBooking);
            tourBookingDto.UserId = userId;

            var createdBooking = await _tourBookingService.AddTourBookingAsync(tourBookingDto);
            return Ok(_mapper.Map<TourBookingResponse>(_mapper));
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

            var tourBookingDto = _mapper.Map<TourBookingDto>(tourBooking);
            tourBookingDto.Id = id;
            tourBookingDto.UserId = userId;

            var updatedBooking = await _tourBookingService.UpdateTourBookingAsync(tourBookingDto);
            if (updatedBooking == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TourBookingResponse>(updatedBooking));
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
