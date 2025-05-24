using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models.Users;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            var user = await _authService.Register(registerDto);
            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var token = await _authService.Login(login.Email, login.Password);

            return Ok(new { Token = token });
        }
    }
}
