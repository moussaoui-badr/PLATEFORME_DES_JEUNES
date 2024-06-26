﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    [Authorize(Roles = "Admin")]
    public class StatistiquesController : Controller
    {
        private readonly IStatistiqueService _statistiqueService;

        public StatistiquesController(IStatistiqueService statistiqueService)
        {
            _statistiqueService = statistiqueService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _statistiqueService.GetStatistique());
        }

        public async Task<IActionResult> P2(string PGFilter, string SearchDate, string SearchDateO, int? Cloture_EnCours)
        {
            ViewData["CurrentFilterD"] = SearchDate;
            ViewData["CurrentFilterDO"] = SearchDateO;
            ViewData["PlateformeGestionnaireSortParm"] = PGFilter;
            ViewData["Cloture_EnCours"] = Cloture_EnCours;
            return View(await _statistiqueService.GetStatistique(PGFilter, SearchDate, SearchDateO, Cloture_EnCours));
        }

        public async Task<IActionResult> P3(string DateDu, string DateAu)
        {
            ViewData["DateDu"] = DateDu;
            ViewData["DateAu"] = DateAu;
            return View(await _statistiqueService.StatistiquesComptabilite(DateDu, DateAu));
        }

    }
}
