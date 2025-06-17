using Travel_agency.Core.BusinessModels.Users;

namespace Travel_agency.BLL.Abstractions
{
    public interface IJWTProvider
    {
        string GenerateToken(UserModel userModel);
    }
}