using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class ComptabiliteRepository : IComptabiliteRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ComptabiliteRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Chapitre>> GetListChapitres(int ChapitreId, double MontantTotal, string Date, int Exercice)
        {
            var result = _dbContext.Chapitres.Include(c => c.Fonctionnements).AsQueryable();
            if(Exercice != 0 && !string.IsNullOrEmpty(Date))
            {
                DateTime oDate = Convert.ToDateTime(Date);
                DateTime premierJourAnnee = new(oDate.Year, 1, 1); // Premier jour de l'année
                DateTime dernierJourAnnee = new(oDate.Year, 12, 31); // Dernier jour de l'année

                result = result.Where(p => p.DateCreation >= premierJourAnnee && p.DateCreation <= dernierJourAnnee);
                return await result.ToListAsync();
            }
            if (ChapitreId != 0)
            {
                result = result.Where(c => c.ChapitreID == ChapitreId);
            }
            if (MontantTotal != 0)
            {
                result = result.Where(c => c.MontantTotale == MontantTotal);
            }
            if (!string.IsNullOrEmpty(Date))
            {
                DateTime oDate = Convert.ToDateTime(Date);
                result = result.Where(p => p.DateCreation == oDate);
            }
            return await result.ToListAsync();
        }

        public async Task<List<Fonctionnement>> GetListFonctionnement(int ChapitreId, string NuméroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            var result = _dbContext.Fonctionnements.Include(c => c.Chapitre).AsQueryable();

            if (ChapitreId != 0)
            {
                result = result.Where(c => c.ChapitreId == ChapitreId);
            }
            if (!string.IsNullOrEmpty(NuméroCheque))
            {
                result = result.Where(c => c.NuméroCheque == NuméroCheque);
            }
            if (!string.IsNullOrEmpty(Beneficiaire))
            {
                result = result.Where(c => c.Beneficiaire == Beneficiaire);
            }
            if (Montant != 0)
            {
                result = result.Where(c => c.Montant == Montant);
            }
            if (!string.IsNullOrEmpty(SearchDate))
            {
                DateTime oDate = Convert.ToDateTime(SearchDate);
                result = result.Where(p => p.Date >= oDate);
            }
            if (!string.IsNullOrEmpty(SearchDateO))
            {
                DateTime oDateO = Convert.ToDateTime(SearchDateO);
                result = result.Where(p => p.Date <= oDateO);
            }
            return await result.ToListAsync();
        }

        public async Task<List<Financement>> GetListFinancement(int type, string Candidat, string NumeroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            var result = _dbContext.Financements.AsQueryable();
            if (type != 0)
            {
                result = result.Where(c => c.TypeFinancement == type);
            }
            if (!string.IsNullOrEmpty(Candidat))
            {
                result = result.Where(c => c.Candidat == Candidat);
            }
            if (!string.IsNullOrEmpty(NumeroCheque))
            {
                result = result.Where(c => c.NumeroCheque == NumeroCheque);
            }
            if (!string.IsNullOrEmpty(Beneficiaire))
            {
                result = result.Where(c => c.Beneficiaire == Beneficiaire);
            }
            if (Montant != 0)
            {
                result = result.Where(c => c.Montant == Montant);
            }
            if (!string.IsNullOrEmpty(SearchDate))
            {
                DateTime oDate = Convert.ToDateTime(SearchDate);
                result = result.Where(p => p.Date >= oDate);
            }
            if (!string.IsNullOrEmpty(SearchDateO))
            {
                DateTime oDateO = Convert.ToDateTime(SearchDateO);
                result = result.Where(p => p.Date <= oDateO);
            }
            return await result.ToListAsync();
        }

        public async Task<bool> AddChapitreByAdmin(Chapitre chapitre)
        {
            await _dbContext.Chapitres.AddAsync(chapitre);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddFonctionnement(Fonctionnement fonctionnement)
        {
            await _dbContext.Fonctionnements.AddAsync(fonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddFinancement(Financement financement)
        {
            await _dbContext.Financements.AddAsync(financement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Chapitre>> GetListChapitres()
        {
            return await _dbContext.Chapitres.Select(c => new Chapitre { ChapitreID = c.ChapitreID, ChapitreTitle = c.ChapitreTitle }).ToListAsync();
        }


        public async Task<bool> DeleteFinancement(int id)
        {
            var financement = await _dbContext.Financements.FindAsync(id);
            _dbContext.Financements.Remove(financement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteFonctionnement(int id)
        {
            var fonctionnement = await _dbContext.Fonctionnements.FindAsync(id);
            _dbContext.Fonctionnements.Remove(fonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteChapitreAndFonctionnementsRelative(int id)
        {
            var chapitre = await _dbContext.Chapitres.FindAsync(id);
            var fonctionnements = await _dbContext.Fonctionnements.Where(f => f.ChapitreId == id).ToListAsync();
            _dbContext.Fonctionnements.RemoveRange(fonctionnements);
            _dbContext.Chapitres.Remove(chapitre);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateFonctionnement(Fonctionnement fonctionnement)
        {
            _dbContext.Fonctionnements.Update(fonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateFinancement(Financement financement)
        {
            _dbContext.Financements.Update(financement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateChapitre(Chapitre chapitre)
        {
            _dbContext.Chapitres.Update(chapitre);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Chapitre> GetChapitreById(int id)
        {
            return await _dbContext.Chapitres.FindAsync(id);
        }

        public async Task<Fonctionnement> GetFonctionnementById(int id)
        {
            return await _dbContext.Fonctionnements.FindAsync(id);
        }

        public async Task<Financement> GetFinancementById(int id)
        {
            var result = await _dbContext.Financements.FindAsync(id);
            return result;
        }

        //CRUD BudgetFinancement
        public async Task<bool> AddBudgetFinancement(BudgetFinancement budgetFinancement)
        {
            await _dbContext.BudgetFinancement.AddAsync(budgetFinancement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateBudgetFinancement(BudgetFinancement budgetFinancement)
        {
            _dbContext.BudgetFinancement.Update(budgetFinancement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBudgetFinancement(int id)
        {
            var budgetFinancement = await _dbContext.BudgetFinancement.FindAsync(id);
            _dbContext.BudgetFinancement.Remove(budgetFinancement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<BudgetFinancement> GetBudgetFinancementById(int id)
        {
            return await _dbContext.BudgetFinancement.FindAsync(id);
        }

        public async Task<List<BudgetFinancement>> BudgetFinancements(string DateBudget, string Emetteur, string NumeroCheque)
        {
            var result = _dbContext.BudgetFinancement.AsQueryable();
            if (DateBudget != null)
            {
                var DateBudgetDate = Convert.ToDateTime(DateBudget);
                result = result.Where(c => c.DateBudget == DateBudgetDate);
            }
            if (!string.IsNullOrEmpty(Emetteur))
            {
                result = result.Where(c => c.EmetteurBudget == Emetteur);
            }
            if (!string.IsNullOrEmpty(NumeroCheque))
            {
                result = result.Where(c => c.NuméroCheque == NumeroCheque);
            }
            return await result.ToListAsync();
        }

        //CRUD Budget Fonctionnement
        public async Task<bool> AddBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement)
        {
            await _dbContext.BudgetFonctionnement.AddAsync(budgetFonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateBudgetFonctionnement(BudgetFonctionnement budgetFonctionnement)
        {
            _dbContext.BudgetFonctionnement.Update(budgetFonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBudgetFonctionnement(int id)
        {
            var budgetFonctionnement = await _dbContext.BudgetFonctionnement.FindAsync(id);
            _dbContext.BudgetFonctionnement.Remove(budgetFonctionnement);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<BudgetFonctionnement> GetBudgetFonctionnementById(int id)
        {
            return await _dbContext.BudgetFonctionnement.FindAsync(id);
        }

        public async Task<List<BudgetFonctionnement>> BudgetFonctionnements(string DateBudget, string Emetteur, string NumeroCheque)
        {
            var result = _dbContext.BudgetFonctionnement.AsQueryable();
            if (DateBudget != null)
            {
                var DateBudgetDate = Convert.ToDateTime(DateBudget);
                result = result.Where(c => c.DateBudget == DateBudgetDate);
            }
            if (!string.IsNullOrEmpty(Emetteur))
            {
                result = result.Where(c => c.EmetteurBudget == Emetteur);
            }
            if (!string.IsNullOrEmpty(NumeroCheque))
            {
                result = result.Where(c => c.NuméroCheque == NumeroCheque);
            }
            return await result.ToListAsync();
        }

    }
}
