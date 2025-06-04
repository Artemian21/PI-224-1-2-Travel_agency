using Travel_agency.Core.BusinessModels.Users;

namespace Travel_agency.BLL.Abstractions
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        Task<UserModel> Register(RegisterUserModel registerModel);
    }
}