using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Models;
using Travel_agency.Core.Models.Users;
using Travel_agency.PL.Models;

namespace Travel_agency.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUserAsync();
            var userResponse = users.Select(user => new UserResponse
            (
                user.Id,
                user.Username,
                user.Email,
                user.Role
            ));

            return Ok(userResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserResponse
            (
                user.Id,
                user.Username,
                user.Email,
                user.Role
            );

            return Ok(userResponse);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserResponse
            (
                user.Id,
                user.Username,
                user.Email,
                user.Role
            );

            return Ok(userResponse);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequest user)
        {
            if (user == null)
            {
                return BadRequest("User data is null.");
            }

            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "nameidentifier")?.Value;

            if (currentUserRole != "Administrator" && currentUserRole != "Manager" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            var userDto = new UserDto
            {
                Id = id,
                Username = user.Username,
                Email = user.Email
            };

            var updatedUser = await _userService.UpdateUserProfileAsync(userDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            var userResponse = new UserResponse
            (
                updatedUser.Id,
                updatedUser.Username,
                updatedUser.Email,
                updatedUser.Role
            );
            return Ok(userResponse);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPut("role/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeUserRole(Guid id, [FromBody] UserRole userRole)
        {
            if (userRole == null)
            {
                return BadRequest("New user role is null.");
            }

            var result = await _userService.AssignUserRoleAsync(id, userRole);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
