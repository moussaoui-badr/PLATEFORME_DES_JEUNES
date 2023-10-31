using Domain.Enums;
using Domain.Models.Authentication;

namespace Repository.IRepositories
{
    public interface IAuthenticationRepository
    {

        Task<Response> Login(LoginModel loginModel);
        Task<Response> Register(RegisterModel registerModel);
        Task<bool> Logout();

    }
}
