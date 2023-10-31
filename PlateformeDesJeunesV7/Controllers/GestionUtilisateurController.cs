using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    [Authorize(Roles = "Admin")]
    public class GestionUtilisateurController : Controller
    {
        private readonly IGestionUtilisateurService _gestionUtilisateurService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public GestionUtilisateurController(IGestionUtilisateurService gestionUtilisateurService, RoleManager<IdentityRole> roleManager)
        {
            _gestionUtilisateurService = gestionUtilisateurService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _gestionUtilisateurService.GetUsersBasicInfo());
        }

        public async Task<IActionResult> AffectationRoles(string userId)
        {
            await DefaultRoles(_roleManager);
            return View(await _gestionUtilisateurService.getUserAndRoles(userId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(UserRolesViewModel model)
        {
            await _gestionUtilisateurService.UpdateRoles(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            await _gestionUtilisateurService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public async Task DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Where(i => i.Name == Roles.Comptable.ToString()).Any())
            {
                //await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.Gestionnaire.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.HeureJoyeuse.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.Amideast.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Comptable.ToString()));
            }

        }
    }
}
