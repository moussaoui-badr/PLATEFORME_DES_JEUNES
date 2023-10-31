using Domain.Entities;
using Domain.Enums;
using Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Repository.Data;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<Response> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName) ?? await _userManager.FindByEmailAsync(loginModel.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                    return new Response { Success = true, Message = "" };
                else
                {
                    if (!user.EmailConfirmed) return new Response { Success = false, Message = "Veuiller confirmer votre email" };
                    else return new Response { Success = false, Message = "Nom d'utilisateur ou mot de passe incorrect" };
                }

            }

            return new Response { Success = false, Message = "Votre compte n'a pas été trouvé" };
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<Response> Register(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            var userNameExists = await _userManager.FindByNameAsync(registerModel.UserName);
            if (userExists != null && userNameExists != null)
            {
                return new Response { Success = false, Message = "Cet Nom d\'utilisateur et cet Email sont déja existant." };
            }
            else if (userExists != null)
            {
                return new Response { Success = false, Message = "Cet Nom d\'utilisateur est déja existant." };
            }
            else if (userNameExists != null)
            {
                return new Response { Success = false, Message = "Cet Nom d\'utilisateur est déja existant." };
            }

            var user = new ApplicationUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                //var resultRole = await _userManager.AddToRoleAsync(user, registerModel.RoleName);

                if (result.Succeeded)
                {
                    return new Response { Success = true, Message = "Ajouté avec succée", Message2 = user.Id };
                }
                else
                {
                    return new Response { Success = false, Message = string.Join(',', result.Errors.Select(c => c.Description)) };
                }
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }



    }
}
