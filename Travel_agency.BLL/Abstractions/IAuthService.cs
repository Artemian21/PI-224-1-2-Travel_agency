using Travel_agency.Core.Models.Users;

namespace Travel_agency.BLL.Abstractions
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        Task<UserDto> Register(RegisterUserDto registerDto);
    }
}