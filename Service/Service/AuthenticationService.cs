using Domain.Enums;
using Domain.Models.Authentication;
using Repository.IRepositories;
using Service.IService;

namespace Service.Service
{
#nullable disable
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<Response> Login(LoginModel loginModel)
        {
            return await _authenticationRepository.Login(loginModel);
        }

        public async Task<bool> Logout()
        {
            return await _authenticationRepository.Logout();
        }

        public async Task<Response> Register(RegisterModel registerModel)
        {
            return await _authenticationRepository.Register(registerModel);
        }


    }
}
