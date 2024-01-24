using Domain.Models;

namespace Service.IService
{
    public interface IStatistiqueService
    {
        Task<StatistiqueP2> GetStatistique(string SearchPG, string SearchDate, string SearchDateO, int? Cloture_EnCours);
        Task<Statistique> GetStatistique();
        Task<StatistiquesComptabilite> StatistiquesComptabilite(string DateDu, string DateAu);

    }
}
