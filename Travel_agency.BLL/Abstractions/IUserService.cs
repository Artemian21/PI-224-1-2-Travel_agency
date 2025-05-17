using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions
{
    public interface IUserService
    {
        Task<bool> AssignUserRoleAsync(Guid userId, string role);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> UpdateUserProfileAsync(UserDto updateDto);
    }
}
