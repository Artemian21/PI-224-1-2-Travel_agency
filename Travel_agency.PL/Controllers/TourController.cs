using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models.Tours;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTours()
        {
            var tours = await _tourService.GetAllToursAsync();
            return Ok(tours);
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedTours([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _tourService.GetPagedToursAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTourById(Guid id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return Ok(tour);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> CreateTour([FromBody] TourRequest tour)
        {
            if (tour == null)
            {
                return BadRequest("Tour data is null.");
            }

            var tourDto = new TourDto
            {
                Name = tour.Name,
                Type = tour.Type,
                Country = tour.Country,
                Region = tour.Region,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                Price = tour.Price,
                ImageUrl = tour.ImageUrl
            };

            var createdTour = await _tourService.AddTourAsync(tourDto);
            return Ok(createdTour);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> UpdateTour(Guid id, [FromBody] TourRequest tour)
        {
            if (tour == null)
            {
                return BadRequest("Tour data is null.");
            }

            var tourDto = new TourDto
            {
                Id = id,
                Name = tour.Name,
                Type = tour.Type,
                Country = tour.Country,
                Region = tour.Region,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                Price = tour.Price,
                ImageUrl = tour.ImageUrl
            };
            var updatedTour = await _tourService.UpdateTourAsync(tourDto);
            if (updatedTour == null)
            {
                return NotFound();
            }
            return Ok(updatedTour);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> DeleteTour(Guid id)
        {
            var deleted = await _tourService.DeleteTourAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
