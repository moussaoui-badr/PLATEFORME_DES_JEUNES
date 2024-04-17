using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Repository.Data
{
    public static class DbInitializer
    {
        public static async Task DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any(i => i.Name == Roles.GestionneurBlog.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.GestionneurBlog.ToString()));
            }

        }

        public static async Task SeedGestionneurBlogAsync(UserManager<IdentityUser> userManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "GestionneurBlog",
                Email = "GestionneurBlog@hotmail.com",
                EmailConfirmed = true,

            };
            var user = userManager.FindByEmailAsync(defaultUser.Email);
            if (user.Result == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin123@");
                await userManager.AddToRoleAsync(defaultUser, Roles.GestionneurBlog.ToString());
            }

        }
    }
}
