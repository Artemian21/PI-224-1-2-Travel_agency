using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Models.Users;

namespace Travel_agency.BLL.Abstractions
{
    public interface IUserService
    {
        Task<bool> AssignUserRoleAsync(Guid userId, UserRole role);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserWithBookingsDto> GetUserByEmailAsync(string email);
        Task<UserWithBookingsDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> UpdateUserProfileAsync(UserDto updateDto);
    }
}
