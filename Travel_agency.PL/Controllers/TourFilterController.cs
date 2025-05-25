using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Models.Tours;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourFilterController : ControllerBase
    {
        private readonly ITourQueryService _tourQueryService;

        public TourFilterController(ITourQueryService tourQueryService)
        {
            _tourQueryService = tourQueryService;
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterTours([FromBody] TourFilterDto tourFilterDto)
        {
            var filteredTours = await _tourQueryService.GetFilteredToursAsync(tourFilterDto);
            return Ok(filteredTours);
        }

        [HttpGet("search/{searchQuary}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchTours(string searchQuary)
        {
            var searchedTours = await _tourQueryService.SearchToursAsync(searchQuary);
            return Ok(searchedTours);
        }
    }
}
