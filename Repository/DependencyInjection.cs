using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Data;
using Repository.IRepositories;
using Repository.Repositories;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ICandidatRepository, CandidatRepository>();
            services.AddScoped<ICandidatPublicRepository, CandidatPublicRepository>();
            services.AddScoped<IGestionCandidatPublicRepository, GestionCandidatPublicRepository>();
            services.AddScoped<IStatistiqueRepository, StatistiqueRepository>();
            services.AddScoped<IFormationRepository, FormationRepository>();
            services.AddScoped<IGestionUtilisateurRepository, GestionUtilisateurRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<ICandidatINDHRepository, CandidatINDHRepository>();
            services.AddScoped<IComptabiliteRepository, ComptabiliteRepository>();
            services.AddScoped<IExerciceRepository, ExerciceRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}