using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class GestionUtilisateurRepository : IGestionUtilisateurRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public GestionUtilisateurRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task UpdateRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
        }

        public async Task<UserRolesViewModel> getUserAndRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };


            return viewModel;
        }

        public async Task<List<UserViewModel>> GetUsersBasicInfo()
        {
            return await _userManager.Users
                .Select(user => new UserViewModel
                {
                    ID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                }).ToListAsync();

        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(user);
        }

        public async Task<List<string>> GetRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}
