using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Repository.Data
{
    public static class DbInitializer
    {
        public static async Task DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Where(i => i.Name == Roles.Comptable.ToString()).Any())
            {
                //await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.Gestionnaire.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.HeureJoyeuse.ToString()));
                //await roleManager.CreateAsync(new IdentityRole(Roles.Amideast.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Comptable.ToString()));
            }

        }

        public static async Task SeedGestionnaireUserAsync(UserManager<IdentityUser> userManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "gestionnaire@hotmail.com",
                Email = "gestionnaire@hotmail.com",
                EmailConfirmed = true,

            };
            var user = userManager.FindByEmailAsync(defaultUser.Email);
            if (user.Result == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin123@");
                await userManager.AddToRoleAsync(defaultUser, Roles.Gestionnaire.ToString());
            }

        }
        public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "admin@hotmail.com",
                Email = "admin@hotmail.com",
                EmailConfirmed = true,

            };
            var user = userManager.FindByEmailAsync(defaultUser.Email);
            if (user.Result == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin123@");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Admin.ToString(), Roles.Gestionnaire.ToString() });
            }
        }

        public static async Task SeedComptableUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "comptable@hotmail.com",
                Email = "comptable@hotmail.com",
                EmailConfirmed = true,

            };
            var user = userManager.FindByEmailAsync(defaultUser.Email);
            if (user.Result == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin123@");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Comptable.ToString() });
            }
        }

    }
}
