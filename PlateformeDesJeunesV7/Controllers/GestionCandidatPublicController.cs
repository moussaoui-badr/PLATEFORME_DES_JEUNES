using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    [Authorize(Roles = "Admin, Gestionnaire, HeureJoyeuse, Amideast")]
    public class GestionCandidatPublicController : Controller
    {
        private readonly IGestionCandidatPublicService _gestionCandidatPublicService;

        public GestionCandidatPublicController(IGestionCandidatPublicService gestionCandidatPublicService)
        {
            _gestionCandidatPublicService = gestionCandidatPublicService;
        }

        public async Task<IActionResult> Index(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            ViewData["CINSortParm"] = SearchCIN;
            ViewData["OrientationSortParm"] = OrientationFilter;
            ViewData["StatutSortParm"] = SearchStatut;
            ViewData["CurrentFilterN"] = SearchNom;
            ViewData["CurrentFilterP"] = SearchPrenom;
            ViewData["CurrentFilterD"] = SearchDate;
            ViewData["CurrentFilterDO"] = SearchDateO;

            return View(await _gestionCandidatPublicService.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, User));
        }
        public async Task<IActionResult> PrintPdf(int id)
        {
            try
            {
                return await _gestionCandidatPublicService.PrintPdf(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { ex.Message });
            }

        }

        public async Task<IActionResult> Details(int Id)
        {
            return View(await _gestionCandidatPublicService.GetCandidat(Id));
        }
        [HttpPost]
        public async Task<string> Valider(int ID)
        {
            await _gestionCandidatPublicService.Valider(ID);
            return "OK";
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int ID)
        {
            await _gestionCandidatPublicService.Delete(ID);
            return RedirectToAction("Index", "GestionCandidatPublic");
        }
    }
}
