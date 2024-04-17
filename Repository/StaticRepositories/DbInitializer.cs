using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StaticRepositories
{
    public static class DbInitializer
    {
        public static async void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Roles.Any(c => c.Name == "GestionneurBlog"))
            {
                return;   // DB has been seeded
            }

            await context.Roles.AddAsync(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "GestionneurBlog", NormalizedName = "GestionneurBlog".ToUpper() });
            await context.SaveChangesAsync();
        }

    }
}
