using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.PL.Models;


namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelRequest hotel)
        {
            if (hotel == null)
            {
                return BadRequest("Invalid hotel data");
            }

            var hotelDto = new HotelDto
            {
                Name = hotel.Name,
                Country = hotel.Country,
                City = hotel.City,
                Address = hotel.Address
            }; 

            var createdHotel = await _hotelService.AddHotelAsync(hotelDto);
            return Ok(createdHotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelRequest hotel)
        {
            if (hotel == null)
            {
                return BadRequest("Invalid hotel data");
            }

            var hotelDto = new HotelDto
            {
                Id = id,
                Name = hotel.Name,
                Country = hotel.Country,
                City = hotel.City,
                Address = hotel.Address
            };

            var updatedHotel = await _hotelService.UpdateHotelAsync(hotelDto);
            if (updatedHotel == null)
            {
                return NotFound();
            }
            return Ok(updatedHotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
    }
}
