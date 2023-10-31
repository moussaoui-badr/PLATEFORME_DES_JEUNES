using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    [Authorize(Roles = "Comptable, Admin")]
    public class ComptabiliteController : Controller
    {
        private readonly IComptabiliteService _comptabiliteService;

        public ComptabiliteController(IComptabiliteService comptabiliteService)
        {
            _comptabiliteService = comptabiliteService;
        }

        public async Task<IActionResult> Chapitres(int ChapitreId, double MontantTotale, string Date)
        {
            ViewBag.Chapitres = new SelectList(await _comptabiliteService.GetListChapitres(), "ChapitreID", "ChapitreTitle");
            if(ChapitreId != 0 ||  MontantTotale != 0 || !string.IsNullOrEmpty(Date))
            {
                ViewData["ChapitreId"] = ChapitreId;
                ViewData["MontantTotale"] = MontantTotale;
                ViewData["Date"] = Date;
                return View(await _comptabiliteService.GetListChapitres(ChapitreId, MontantTotale, Date));
            }
            return View(new List<Chapitre>());
        }

        public async Task<IActionResult> Financements(int Type, string Candidat, string NumeroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            if (Type != 0 || !string.IsNullOrEmpty(Candidat) || !string.IsNullOrEmpty(NumeroCheque) ||
            !string.IsNullOrEmpty(Beneficiaire) || Montant != 0.0 || !string.IsNullOrEmpty(SearchDate) ||
            !string.IsNullOrEmpty(SearchDateO))
            {
                ViewData["Candidat"] = Candidat;
                ViewData["NumeroCheque"] = NumeroCheque;
                ViewData["Beneficiaire"] = Beneficiaire;
                ViewData["Montant"] = Montant;
                ViewData["CurrentFilterD"] = SearchDate;
                ViewData["CurrentFilterDO"] = SearchDateO;
                ViewData["Type"] = Type;
                return View(await _comptabiliteService.GetListFinancement(Type, Candidat, NumeroCheque, Beneficiaire, Montant, SearchDate, SearchDateO));
            }
            return View(new List<Financement>());
        }

        public async Task<IActionResult> Fonctionnements(int ChapitreId, string NuméroCheque, string Beneficiaire, double Montant, string SearchDate, string SearchDateO)
        {
            ViewBag.Chapitres = new SelectList(await _comptabiliteService.GetListChapitres(), "ChapitreID", "ChapitreTitle");

            if (ChapitreId != 0 || !string.IsNullOrEmpty(NuméroCheque) ||
    !string.IsNullOrEmpty(Beneficiaire) || Montant != 0.0 ||
    !string.IsNullOrEmpty(SearchDate) || !string.IsNullOrEmpty(SearchDateO))
            {
                //Envoyer vers ma select list les chapitres
                ViewBag.Chapitres = new SelectList(await _comptabiliteService.GetListChapitres(), "ChapitreID", "ChapitreTitle");

                ViewData["ChapitreId"] = ChapitreId;
                ViewData["NuméroCheque"] = NuméroCheque;
                ViewData["Beneficiaire"] = Beneficiaire;
                ViewData["Montant"] = Montant;
                ViewData["CurrentFilterD"] = SearchDate;
                ViewData["CurrentFilterDO"] = SearchDateO;
                return View(await _comptabiliteService.GetListFonctionnement(ChapitreId, NuméroCheque, Beneficiaire, Montant, SearchDate, SearchDateO));
            }
            return View(new List<Fonctionnement>());
        }

        //[Authorize(Roles = "Comptable")]
        public async Task<IActionResult> Add()
        {
            //Envoyer vers ma select list les chapitres
            ViewBag.Chapitres = new SelectList(await _comptabiliteService.GetListChapitres(), "ChapitreID", "ChapitreTitle");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ComptabiliteModel model)
        {
            if (model.Financement != null)
            {
                await _comptabiliteService.AddFinancement(model.Financement);
                return RedirectToAction("Financements");
            }
            else
            {
                await _comptabiliteService.AddFonctionnement(model.Fonctionnement);
                return RedirectToAction("Fonctionnements");
            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddChapitre(string success)
        {
            if (success != null)
            {
                ViewBag.Success = "1";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChapitreAsync(Chapitre chapitre)
        {
            await _comptabiliteService.AddChapitreByAdmin(chapitre);
            return RedirectToAction("AddChapitre", new { success = "1" });
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFonctionnement(int id)
        {
            await _comptabiliteService.DeleteFonctionnement(id);
            return RedirectToAction("Fonctionnements");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFinancement(int id)
        {
            await _comptabiliteService.DeleteFinancement(id);
            return RedirectToAction("Financements");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveChapitre(int id)
        {
            await _comptabiliteService.DeleteChapitreAndFonctionnementsRelative(id);
            return RedirectToAction("Chapitres");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFonctionnement(int id)
        {
            //Envoyer vers ma select list les chapitres
            ViewBag.Chapitres = new SelectList(await _comptabiliteService.GetListChapitres(), "ChapitreID", "ChapitreTitle");
            return View(await _comptabiliteService.GetFonctionnementById(id));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFinancement(int id)
        {
            return View(await _comptabiliteService.GetFinancementById(id));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateChapitre(int id)
        {
            return View(await _comptabiliteService.GetChapitreById(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFonctionnement(Fonctionnement model)
        {
            await _comptabiliteService.UpdateFonctionnement(model);
            return RedirectToAction("Fonctionnements");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFinancement(Financement model)
        {
            await _comptabiliteService.UpdateFinancement(model);
            return RedirectToAction("Financements");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateChapitre(Chapitre chapitre)
        {
            await _comptabiliteService.UpdateChapitre(chapitre);
            return RedirectToAction("Chapitres");
        }

        //CRUD Budget financement
        public async Task<IActionResult> BudgetFinancement(string DateBudget, string Emetteur, string NumeroCheque)
        {
            return View(await _comptabiliteService.BudgetFinancements(DateBudget, Emetteur, NumeroCheque));
        }

        public IActionResult AddBudgetFinancement()
        {
            return View();
        }

        public async Task<IActionResult> UpdateBudgetFinancement(int id)
        {
            return View(await _comptabiliteService.GetBudgetFinancementById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddBudgetFinancement(BudgetFinancement model)
        {
            await _comptabiliteService.AddBudgetFinancement(model);
            return RedirectToAction("BudgetFinancement");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBudgetFinancement(BudgetFinancement model)
        {
            await _comptabiliteService.UpdateBudgetFinancement(model);
            return RedirectToAction("BudgetFinancement");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveBudgetFinancement(int id)
        {
            await _comptabiliteService.DeleteBudgetFinancement(id);
            return RedirectToAction("BudgetFinancement");
        }

        //CRUD Budget fonctionnement
        public async Task<IActionResult> BudgetFonctionnement(string DateBudget, string Emetteur, string NumeroCheque)
        {
            return View(await _comptabiliteService.BudgetFonctionnements(DateBudget, Emetteur, NumeroCheque));
        }
        public IActionResult AddBudgetFonctionnement()
        {
            return View();
        }
        public async Task<IActionResult> UpdateBudgetFonctionnement(int id)
        {
            return View(await _comptabiliteService.GetBudgetFonctionnementById(id));
        }


        [HttpPost]
        public async Task<IActionResult> AddBudgetFonctionnement(BudgetFonctionnement model)
        {
            await _comptabiliteService.AddBudgetFonctionnement(model);
            return RedirectToAction("BudgetFonctionnement");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBudgetFonctionnement(BudgetFonctionnement model)
        {
            await _comptabiliteService.UpdateBudgetFonctionnement(model);
            return RedirectToAction("BudgetFonctionnement");
        }
        [HttpGet]
        public async Task<IActionResult> RemoveBudgetFonctionnement(int id)
        {
            await _comptabiliteService.DeleteBudgetFonctionnement(id);
            return RedirectToAction("BudgetFonctionnement");
        }

    }
}
