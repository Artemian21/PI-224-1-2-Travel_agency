using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Transports;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketBookingController : ControllerBase
    {
        private readonly ITicketBookingService _ticketBookingService;

        public TicketBookingController(ITicketBookingService ticketBookingService)
        {
            _ticketBookingService = ticketBookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _ticketBookingService.GetAllTicketBookingsAsync();
            return Ok(bookings);
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

            var ticketBookingDto = new TicketBookingDto
            {
                UserId = ticketBooking.UserId,
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

            var ticketBookingDto = new TicketBookingDto
            {
                Id = id,
                UserId = ticketBooking.UserId,
                TransportId = ticketBooking.TransportId,
                Status = ticketBooking.Status
            };
            var updatedBooking = await _ticketBookingService.UpdateTicketBookingAsync(ticketBookingDto);
            if (updatedBooking == null)
            {
                return NotFound();
            }
            return Ok(updatedBooking);
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
