using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using Travel_agency.BLL.Abstractions;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Transports;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTransports()
        {
            var transports = await _transportService.GetAllTransportsAsync();
            return Ok(transports);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTransportById(Guid id)
        {
            var transport = await _transportService.GetTransportByIdAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            return Ok(transport);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> CreateTransport([FromBody] TransportRequest transport)
        {
            if (transport == null)
            {
                return BadRequest("Transport data is null.");
            }

            var transportDto = new TransportDto
            {
                Type = transport.Type,
                Company = transport.Company,
                DepartureDate = transport.DepartureDate,
                ArrivalDate = transport.ArrivalDate,
                Price = transport.Price
            };

            var createdTransport = await _transportService.AddTransportAsync(transportDto);
            return Ok(createdTransport);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> UpdateTransport(Guid id, [FromBody] TransportRequest transport)
        {
            if (transport == null)
            {
                return BadRequest("Transport data is null.");
            }
            
            var transportDto = new TransportDto
            {
                Id = id,
                Type = transport.Type,
                Company = transport.Company,
                DepartureDate = transport.DepartureDate,
                ArrivalDate = transport.ArrivalDate,
                Price = transport.Price
            };

            var updatedTransport = await _transportService.UpdateTransportAsync(transportDto);
            if (updatedTransport == null)
            {
                return NotFound();
            }
            return Ok(updatedTransport);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> DeleteTransport(Guid id)
        {
            var transport = await _transportService.GetTransportByIdAsync(id);
            if (transport == null)
            {
                return NotFound();
            }

            await _transportService.DeleteTransportAsync(id);
            return NoContent();
        }
    }
}
