using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    [Authorize(Roles = "Admin, Gestionnaire, HeureJoyeuse, Amideast")]
    public class FormationsController : Controller
    {
        private readonly IFormationService _formationService;

        public FormationsController(IFormationService formationService)
        {
            _formationService = formationService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string Theme, string Animateur, int Duree, string SearchDate, string SearchDateO)
        {
            return View(await _formationService.getFormations(pageNumber, User, Theme, Animateur, Duree, SearchDate, SearchDateO));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _formationService.getFormation(id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Formation formation)
        {
            await _formationService.AddFormation(formation);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            return View(await _formationService.getFormation(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Formation formation)
        {
            await _formationService.Edit(formation);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            await _formationService.Delete(Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<string> EditAffectation(int formationId, int clientId, bool valide)
        {
            await _formationService.EditAffectation(formationId, clientId, valide);
            return "OK";
        }

        [HttpGet]
        public async Task<IActionResult> Candidats(int FormationID, string Theme, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            if (FormationID != 0)
            {
                ViewData["FormationID"] = FormationID;
            }

            ViewData["OrientationSortParm"] = OrientationFilter;
            ViewData["Theme"] = Theme;
            ViewData["CINSortParm"] = SearchCIN;
            ViewData["StatutSortParm"] = SearchStatut;
            ViewData["CurrentFilterN"] = SearchNom;
            ViewData["CurrentFilterP"] = SearchPrenom;
            ViewData["CurrentFilterD"] = SearchDate;
            ViewData["CurrentFilterDO"] = SearchDateO;


            return View(await _formationService.getCandidats(FormationID, Theme, SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, User));
        }
    }
}
