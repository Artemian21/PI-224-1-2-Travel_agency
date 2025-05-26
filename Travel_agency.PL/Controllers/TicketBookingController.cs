using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Transports;
using Travel_agency.PL.Models.Requests;
using Travel_agency.PL.Models.Responses;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketBookingController : ControllerBase
    {
        private readonly ITicketBookingService _ticketBookingService;
        private readonly IMapper _mapper;

        public TicketBookingController(ITicketBookingService ticketBookingService, IMapper mapper)
        {
            _ticketBookingService = ticketBookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var allBookings = await _ticketBookingService.GetAllTicketBookingsAsync();

            if (User.IsInRole("Admin"))
            {
                return Ok(_mapper.Map<List<TicketBookingResponse>>(allBookings));
            }
            else
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value);
                var userBookings = allBookings.Where(b => b.UserId == userId).ToList();
                return Ok(_mapper.Map<List<TicketBookingResponse>>(userBookings));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketBookingById(Guid id)
        {
            var booking = await _ticketBookingService.GetTicketBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicketBooking([FromBody] TicketBookingRequest ticketBooking)
        {
            if (ticketBooking == null)
            {
                return BadRequest("Ticket booking data is null.");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var ticketBookingDto = new TicketBookingDto
            {
                UserId = userId,
                TransportId = ticketBooking.TransportId,
                Status = ticketBooking.Status
            };

            var createdBooking = await _ticketBookingService.AddTicketBookingAsync(ticketBookingDto);
            return CreatedAtAction(nameof(GetTicketBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicketBooking(Guid id, [FromBody] TicketBookingRequest ticketBooking)
        {
            if (ticketBooking == null)
            {
                return BadRequest("Ticket booking data is null.");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var ticketBookingDto = _mapper.Map<TicketBookingDto>(ticketBooking);
            ticketBookingDto.Id = id;
            ticketBookingDto.UserId = userId;


            var updatedBooking = await _ticketBookingService.UpdateTicketBookingAsync(ticketBookingDto);
            if (updatedBooking == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<HotelRoomResponse>(updatedBooking));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketBooking(Guid id)
        {
            var deleted = await _ticketBookingService.DeleteTicketBookingAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
