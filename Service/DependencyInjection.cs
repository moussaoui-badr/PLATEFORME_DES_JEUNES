using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.IService;
using Service.Service;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
            serviceCollection.AddScoped<ICandidatService, CandidatService>();
            serviceCollection.AddScoped<ICandidatPublicService, CandidatPublicService>();
            serviceCollection.AddScoped<IGestionCandidatPublicService, GestionCandidatPublicService>();
            serviceCollection.AddScoped<IStatistiqueService, StatistiqueService>();
            serviceCollection.AddScoped<IFormationService, FormationService>();
            serviceCollection.AddScoped<IGestionUtilisateurService, GestionUtilisateurService>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddOptions<CaptchaSettings>().BindConfiguration("Captcha");
            serviceCollection.AddScoped<IRecaptchaService, RecaptchaService>();
            serviceCollection.AddScoped<ICandidatINDHService, CandidatINDHService>();
            serviceCollection.AddScoped<IComptabiliteService, ComptabiliteService>();

            return serviceCollection;
        }

        public static char GetFirstChar(this string str)
        {
            return str[0];
        }
    }
}