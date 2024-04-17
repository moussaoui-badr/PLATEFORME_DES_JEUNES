using Domain.Enums;
using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Repository.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}
