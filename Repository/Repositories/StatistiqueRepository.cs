using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Data.Migrations;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class StatistiqueRepository : IStatistiqueRepository
    {
        private readonly ApplicationDbContext _context;

        public StatistiqueRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<StatistiqueP2> GetStatistique(string SearchPG, string SearchDate, string SearchDateO, int? Cloture_EnCours)
        {
            var Montants = _context.ClientFinances.AsQueryable();
            var INDH = _context.INDHS.Where(c => c.ClientFinanceID != null && c.ClientFinanceID != 0).AsQueryable();
            var NombreCandidats = await Montants.CountAsync();

            //if (Cloture_EnCours != null)
            //{
            //    if (Cloture_EnCours == 2)
            //    {
            //        // Condition pour les dossiers en cours
            //        Montants = Montants.Where(c =>
            //            Math.Abs(c.MontantProjet - (c.INDHS.Select(m => m.PartIndh).Sum() +
            //            c.INDHS.Select(m => m.ApportEnDhs).Sum() +
            //            c.INDHS.Select(m => m.ApportEnAmenagement).Sum())) != 0);
            //    }
            //    else if (Cloture_EnCours == 1)
            //    {
            //        Montants = Montants.Where(c =>
            //            Math.Abs(c.MontantProjet - (c.INDHS.Select(m => m.PartIndh).Sum() +
            //            c.INDHS.Select(m => m.ApportEnDhs).Sum() +
            //            c.INDHS.Select(m => m.ApportEnAmenagement).Sum())) == 0);
            //    }
            //}
            if (!string.IsNullOrEmpty(SearchPG))
            {
                Montants = Montants.Where(c => c.PlateformeGestionnaire == (SearchPG == "AinSebaa" ? PlateformeGestionnaire.AinSebaa : SearchPG == "RocheNoir" ? PlateformeGestionnaire.RocheNoir : PlateformeGestionnaire.HayMohammadi));
                INDH = INDH.Where(c => c.PlateformeGestionnaire == (SearchPG == "AinSebaa" ? PlateformeGestionnaire.AinSebaa : SearchPG == "RocheNoir" ? PlateformeGestionnaire.RocheNoir : PlateformeGestionnaire.HayMohammadi));
                NombreCandidats = await Montants.CountAsync();
            }
            if (!string.IsNullOrEmpty(SearchDate))
            {
                DateTime oDate = Convert.ToDateTime(SearchDate);
                Montants = Montants.Where(p => p.Created >= oDate);
                INDH = INDH.Where(p => p.Created >= oDate);
                NombreCandidats = await Montants.Where(p => p.Created >= oDate).CountAsync();
            }
            if (!string.IsNullOrEmpty(SearchDateO))
            {
                DateTime oDateO = Convert.ToDateTime(SearchDateO);
                Montants = Montants.Where(p => p.Created <= oDateO);
                INDH = INDH.Where(p => p.Created <= oDateO);
                NombreCandidats = await Montants.Where(p => p.Created <= oDateO).CountAsync();
            }

            var MontantProjet = await Montants.Select(m => m.MontantProjet).SumAsync();
            var MontantApportPersonnel = await Montants.Select(m => m.MontantApportPersonnel).SumAsync();
            var MontantINDH = await Montants.Select(m => m.MontantINDH).SumAsync();

            var MontantAcquisition = await INDH.SumAsync(m => m.MontantAcquisition);
            var PartINDH = await INDH.SumAsync(m => m.PartIndh);
            var ApportEnDHS = await INDH.SumAsync(m => m.ApportEnDhs);
            var ApportEnAmenagement = await INDH.SumAsync(m => m.ApportEnAmenagement);
            var Autre = await INDH.SumAsync(m => m.Autre);

            var MontantMaterielAcquis = await INDH.Where(c => c.Etat == Etat.Acquis).Select(m => m.MontantAcquisition).SumAsync();
            var MontantMaterielECA = await INDH.Where(c => c.Etat == Etat.EnCoursAcquisition).Select(m => m.MontantAcquisition).SumAsync();
            var MontantMaterielAmenagement = await INDH.Where(c => c.Etat == Etat.Amenagement).Select(m => m.MontantAcquisition).SumAsync();

            var NombreMaterielAcquis = await INDH.Where(c => c.Etat == Etat.Acquis).Select(m => m.Materiel).CountAsync();
            var NombreMaterielECA = await INDH.Where(c => c.Etat == Etat.EnCoursAcquisition).Select(m => m.Materiel).CountAsync();
            var NombreMaterielAmenagement = await INDH.Where(c => c.Etat == Etat.Amenagement).Select(m => m.Materiel).CountAsync();

            return new StatistiqueP2
            {
                NombreCandidat = NombreCandidats.ToString(),

                MontantProjet = MontantProjet,
                MontantApportPersonnel = MontantApportPersonnel,
                MontantINDH = MontantINDH,

                MontantAcquisition = MontantAcquisition,
                PartINDH = PartINDH,
                ApportEnDHS = ApportEnDHS,
                ApportEnAmenagement = ApportEnAmenagement,
                Autre = Autre,

                MontantMaterielAcquis = MontantMaterielAcquis,
                MontantMaterielECA = MontantMaterielECA,
                MontantMaterielAmenagement = MontantMaterielAmenagement,

                NombreMaterielAcquis = NombreMaterielAcquis,
                NombreMaterielECA = NombreMaterielECA,
                NombreMaterielAmenagement = NombreMaterielAmenagement,
            };
        }

        public async Task<StatistiquesComptabilite> StatistiquesComptabilite(string DateDu, string DateAu)
        {
            var Fonctionnements = _context.Fonctionnements.AsQueryable();
            var Financements = _context.Financements.AsQueryable();
            var Chapitres = _context.Chapitres.AsQueryable();
            var BFonctionnements = _context.BudgetFonctionnement.AsQueryable();
            var BFinancements = _context.BudgetFinancement.AsQueryable();

            if (DateDu != null)
            {
                DateTime oDate = Convert.ToDateTime(DateDu);
                Fonctionnements = Fonctionnements.Where(p => p.DateCreation >= oDate);
                Financements = Financements.Where(p => p.DateCreation >= oDate);
                Chapitres = Chapitres.Where(p => p.DateCreation >= oDate);
                BFonctionnements = BFonctionnements.Where(p => p.DateBudget >= oDate);
                BFinancements = BFinancements.Where(p => p.DateBudget >= oDate);
            }
            if (DateAu != null)
            {
                DateTime oDate = Convert.ToDateTime(DateAu);
                Fonctionnements = Fonctionnements.Where(p => p.DateCreation <= oDate);
                Financements = Financements.Where(p => p.DateCreation <= oDate);
                Chapitres = Chapitres.Where(p => p.DateCreation <= oDate);
                BFonctionnements = BFonctionnements.Where(p => p.DateBudget <= oDate);
                BFinancements = BFinancements.Where(p => p.DateBudget <= oDate);
            }

            var MontantChapitre = await Chapitres.Select(m => m.MontantTotale).SumAsync();
            var MontantFonctionnement = await Fonctionnements.Select(c => c.Montant).SumAsync();
            var MontantFinancementDebit = await Financements.Where(t => t.TypeFinancement == 1).Select(c => c.Montant).SumAsync();
            var MontantFinancementCredit = await Financements.Where(t => t.TypeFinancement == 2).Select(c => c.Montant).SumAsync();
            var MontantBudgetFonctionnement = await BFonctionnements.Select(c => c.MontantBudget).SumAsync();
            var MontantBudgetFinancement = await BFinancements.Select(c => c.MontantBudget).SumAsync();

            var ResteFonctionnement = MontantBudgetFonctionnement - MontantFonctionnement;
            var ResteFinancement = (MontantBudgetFinancement - MontantFinancementDebit) + MontantFinancementCredit;

            var NombreFonctionnement = Fonctionnements.Count();
            var NombreFinancementDebit = Financements.Where(t => t.TypeFinancement == 1).Count();
            var NombreFinancementCredit = Financements.Where(t => t.TypeFinancement == 2).Count();
            var NombreChapitre = Chapitres.Count();

            return new StatistiquesComptabilite
            {
                MontantChapitre = MontantChapitre,
                MonantFonctionnement = MontantFonctionnement,
                MontantFinancementDebit = MontantFinancementDebit,
                MontantFinancementCredit = MontantFinancementCredit,
                MontantBudgetFonctionnement = MontantBudgetFonctionnement,
                MontantBudgetFinancement = MontantBudgetFinancement,

                ResteFonctionnement = ResteFonctionnement,
                ResteFinancement = ResteFinancement,

                NombreFonctionnement = NombreFonctionnement,
                NombreFinancementDebit = NombreFinancementDebit,
                NombreFinancementCredit = NombreFinancementCredit,
                NombreChapitre = NombreChapitre
            };
        }

        public async Task<Statistique> GetStatistique()
        {
            var candidatAcceuilli = await _context.Clients.Where(p => p.Public == false).CountAsync();
            var nombreFemmes = await _context.Clients.Where(f => f.Sexe == Sexe.Femme).Where(p => p.Public == false).CountAsync();
            var nombreHommes = await _context.Clients.Where(f => f.Sexe == Sexe.Homme).Where(p => p.Public == false).CountAsync();
            var nombrePorteurDeProjets = await _context.Clients.Where(p => p.Statut == Statut.PorteurDeProjet).Where(p => p.Public == false).CountAsync();
            var nombreChercheurEmploit = await _context.Clients.Where(p => p.Statut == Statut.ChercheurEmploit).Where(p => p.Public == false).CountAsync();
            var nombreFormation = await _context.Formations.CountAsync();
            var nombreCandidatAcceuilliEtOriente = await _context.Clients.Where(o => o.Orientation == Orientation.Orienté).CountAsync();
            var nombreCandidatRefusé = await _context.Clients.Where(o => o.Orientation == Orientation.Refusé).CountAsync();
            var nombreCandidatNonOrienté = await _context.Clients.Where(o => o.Orientation == Orientation.NonOrienté).CountAsync();
            return new Statistique
            {
                candidatAcceuilli = candidatAcceuilli + 302,
                nombreFemmes = nombreFemmes + 102,
                nombreHommes = nombreHommes + 200,
                nombreChercheurEmploit = nombreChercheurEmploit + 130,
                nombrePorteurDeProjets = nombrePorteurDeProjets + 172,
                nombreFormation = nombreFormation,
                nombreCandidatAcceuilliEtOriente = nombreCandidatAcceuilliEtOriente + 107,
                nombreCandidatRefusé = nombreCandidatRefusé + 71,
                nombreCandidatNonOrienté = nombreCandidatNonOrienté + 102
            };
        }

    }
}
