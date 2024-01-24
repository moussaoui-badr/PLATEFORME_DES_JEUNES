using Domain.Models;

namespace Repository.IRepositories
{
    public interface IStatistiqueRepository
    {
        Task<StatistiqueP2> GetStatistique(string SearchPG, string SearchDate, string SearchDateO, int? Cloture_EnCours);
        Task<Statistique> GetStatistique();
        Task<StatistiquesComptabilite> StatistiquesComptabilite(string DateDu, string DateAu);
    }
}
