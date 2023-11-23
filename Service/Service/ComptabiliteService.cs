using Domain.Entities;
using Repository.IRepositories;
using Service.IService;

namespace Service.Service
{
#nullable disable
    public class ComptabiliteService : IComptabiliteService
    {
        private readonly IComptabiliteRepository _comptabiliteRepository;

        public ComptabiliteService(IComptabiliteRepository comptabiliteRepository)
        {
            _comptabiliteRepository = comptabiliteRepository;
        }

        public async Task<bool> AddBudgetFinancement(BudgetFinancement BudgetFinancement)
        {
            return await _comptabiliteRepository.AddBudgetFinancement(BudgetFinancement);
        }

        public async Task<bool> AddBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement)
        {
            return await _comptabiliteRepository.AddBudgetFonctionnement(budgetFonctionnement);
        }

        public async Task<bool> AddChapitreByAdmin(Chapitre chapitre)
        {
            return await _comptabiliteRepository.AddChapitreByAdmin(chapitre);
        }

        public async Task<bool> AddFinancement(Financement financement)
        {
            return await _comptabiliteRepository.AddFinancement(financement);
        }

        public async Task<bool> AddFonctionnement(Fonctionnement fonctionnement)
        {
            return await _comptabiliteRepository.AddFonctionnement(fonctionnement);
        }

        public async Task<List<BudgetFinancement>> BudgetFinancements(string DateBudget, string Emetteur, string NumeroCheque)
        {
            return await _comptabiliteRepository.BudgetFinancements(DateBudget, Emetteur, NumeroCheque);
        }

        public async Task<List<BudgetFonctionnement>> BudgetFonctionnements(string DateBudget, string Emetteur, string NumeroCheque)
        {
            return await _comptabiliteRepository.BudgetFonctionnements(DateBudget, Emetteur, NumeroCheque);
        }

        public async Task<bool> DeleteBudgetFinancement(int id)
        {
            return await _comptabiliteRepository.DeleteBudgetFinancement(id);
        }

        public async Task<bool> DeleteBudgetFonctionnement(int id)
        {
            return await _comptabiliteRepository.DeleteBudgetFonctionnement(id);
        }

        public async Task<bool> DeleteChapitreAndFonctionnementsRelative(int id)
        {
            return await _comptabiliteRepository.DeleteChapitreAndFonctionnementsRelative(id);
        }

        public async Task<bool> DeleteFinancement(int id)
        {
            return await _comptabiliteRepository.DeleteFinancement(id);
        }

        public async Task<bool> DeleteFonctionnement(int id)
        {
            return await _comptabiliteRepository.DeleteFonctionnement(id);
        }

        public async Task<BudgetFinancement> GetBudgetFinancementById(int id)
        {
            return await _comptabiliteRepository.GetBudgetFinancementById(id);
        }

        public async Task<BudgetFonctionnement> GetBudgetFonctionnementById(int id)
        {
            return await _comptabiliteRepository.GetBudgetFonctionnementById(id);
        }

        public async Task<Chapitre> GetChapitreById(int id)
        {
            return await _comptabiliteRepository.GetChapitreById(id);
        }

        public async Task<Financement> GetFinancementById(int id)
        {
            return await _comptabiliteRepository.GetFinancementById(id);
        }

        public async Task<Fonctionnement> GetFonctionnementById(int id)
        {
            return await _comptabiliteRepository.GetFonctionnementById(id);
        }

        public async Task<List<Chapitre>> GetListChapitres(int ChapitreId, double MontantTotal, string Date, int Exercice)
        {
            return await _comptabiliteRepository.GetListChapitres(ChapitreId, MontantTotal, Date, Exercice);
        }

        public async Task<List<Chapitre>> GetListChapitres()
        {
            return await _comptabiliteRepository.GetListChapitres();
        }

        public async Task<List<Financement>> GetListFinancement(int type, string Candidat, string NumeroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            return await _comptabiliteRepository.GetListFinancement(type, Candidat, NumeroCheque, Beneficiaire, Montant, SearchDate, SearchDateO);
        }

        public async Task<List<Fonctionnement>> GetListFonctionnement(int ChapitreId, string NuméroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            return await _comptabiliteRepository.GetListFonctionnement(ChapitreId, NuméroCheque, Beneficiaire, Montant, SearchDate, SearchDateO);
        }

        public async Task<bool> UpdateBudgetFinancement(BudgetFinancement budgetFinancement)
        {
            return await _comptabiliteRepository.UpdateBudgetFinancement(budgetFinancement);
        }

        public async Task<bool> UpdateBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement)
        {
            return await _comptabiliteRepository.UpdateBudgetFonctionnement(budgetFonctionnement);
        }

        public async Task<bool> UpdateChapitre(Chapitre chapitre)
        {
            return await _comptabiliteRepository.UpdateChapitre(chapitre);
        }

        public async Task<bool> UpdateFinancement(Financement financement)
        {
            return await _comptabiliteRepository.UpdateFinancement(financement);
        }

        public async Task<bool> UpdateFonctionnement(Fonctionnement fonctionnement)
        {
            return await _comptabiliteRepository.UpdateFonctionnement(fonctionnement);
        }
    }
}
