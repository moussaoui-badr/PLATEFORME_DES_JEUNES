using Domain.Models;
using Repository.IRepositories;
using Service.IService;

namespace Service.Service
{
    public class StatistiqueService : IStatistiqueService
    {
        private readonly IStatistiqueRepository _statistiqueRepository;

        public StatistiqueService(IStatistiqueRepository statistiqueRepository)
        {
            _statistiqueRepository = statistiqueRepository;
        }

        public async Task<Statistique> GetStatistique()
        {
            return await _statistiqueRepository.GetStatistique();
        }

        public async Task<StatistiqueP2> GetStatistique(string SearchPG, string SearchDate, string SearchDateO)
        {
            return await _statistiqueRepository.GetStatistique(SearchPG, SearchDate, SearchDateO);
        }

        public async Task<StatistiquesComptabilite> StatistiquesComptabilite(string DateDu, string DateAu)
        {
            return await _statistiqueRepository.StatistiquesComptabilite(DateDu, DateAu);
        }
    }
}
