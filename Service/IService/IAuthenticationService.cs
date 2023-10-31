using Domain.Enums;
using Domain.Models.Authentication;

namespace Service.IService
{
    public interface IAuthenticationService
    {
        Task<Response> Login(LoginModel loginModel);
        Task<bool> Logout();
        Task<Response> Register(RegisterModel registerModel);
    }
}
