using Travel_agency.Core.Models.Users;

namespace Travel_agency.BLL.Abstractions
{
    public interface IJWTProvider
    {
        string GenerateToken(UserDto userDto);
    }
}