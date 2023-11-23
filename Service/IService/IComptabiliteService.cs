using Domain.Entities;

namespace Service.IService
{
    public interface IComptabiliteService
    {
        Task<bool> AddChapitreByAdmin(Chapitre chapitre);
        Task<bool> AddFonctionnement(Fonctionnement fonctionnement);
        Task<bool> AddFinancement(Financement financement);

        Task<List<Chapitre>> GetListChapitres(int ChapitreId, double MontantTotal, string Date, int Exercice);
        Task<List<Fonctionnement>> GetListFonctionnement(int ChapitreId, string NuméroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO);
        Task<List<Financement>> GetListFinancement(int type, string Candidat, string NumeroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO);
        Task<List<Chapitre>> GetListChapitres();
        Task<bool> DeleteFinancement(int id);
        Task<bool> DeleteFonctionnement(int id);
        Task<bool> DeleteChapitreAndFonctionnementsRelative(int id);
        Task<bool> UpdateFonctionnement(Fonctionnement fonctionnement);
        Task<bool> UpdateFinancement(Financement financement);
        Task<bool> UpdateChapitre(Chapitre chapitre);
        Task<Chapitre> GetChapitreById(int id);
        Task<Fonctionnement> GetFonctionnementById(int id);
        Task<Financement> GetFinancementById(int id);

        Task<bool> AddBudgetFinancement(BudgetFinancement BudgetFinancement);
        Task<bool> UpdateBudgetFinancement(BudgetFinancement budgetFinancement);
        Task<bool> DeleteBudgetFinancement(int id);
        Task<BudgetFinancement> GetBudgetFinancementById(int id);
        Task<List<BudgetFinancement>> BudgetFinancements(string DateBudget, string Emetteur, string NumeroCheque);

        Task<bool> AddBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement);
        Task<bool> UpdateBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement);
        Task<bool> DeleteBudgetFonctionnement(int id);
        Task<BudgetFonctionnement> GetBudgetFonctionnementById(int id);
        Task<List<BudgetFonctionnement>> BudgetFonctionnements(string DateBudget, string Emetteur, string NumeroCheque);
    }
}
