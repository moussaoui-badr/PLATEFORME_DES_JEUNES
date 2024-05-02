using Domain.Models.Election;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;

namespace Web.Controllers
{
    public class StatistiqueFamilleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public StatistiqueFamilleController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public async Task<IActionResult> Index(StatistiqueFamille model)
        {
            try
            {
                var pivotParSecteurQuery = _context.PersonnePivot.AsQueryable();
                var responsableParSecteurQuery = _context.PersonneResponsable.AsQueryable();
                var membreParSecteurQuery = _context.PersonneMembre.AsQueryable();

                if (model?.SecteurId != null)
                {
                    pivotParSecteurQuery = pivotParSecteurQuery.Where(p => p.SecteurId == model.SecteurId);
                    responsableParSecteurQuery = responsableParSecteurQuery.Where(p => p.SecteurId == model.SecteurId);
                    membreParSecteurQuery = membreParSecteurQuery.Where(p => p.SecteurId == model.SecteurId);
                }

                if (model?.PivotId != null)
                {
                    pivotParSecteurQuery = pivotParSecteurQuery.Where(p => p.PersonnePivotId == model.PivotId);
                    responsableParSecteurQuery = responsableParSecteurQuery.Where(p => p.PivotId == model.PivotId);
                    membreParSecteurQuery = membreParSecteurQuery.Where(p => p.PivotId == model.PivotId);
                }

                model.NombrePivot = await pivotParSecteurQuery.CountAsync();
                model.NombreResponsable = await responsableParSecteurQuery.CountAsync();
                model.NombreMembre = await membreParSecteurQuery.CountAsync();

                var secteurs = await _context.Secteurs.ToListAsync();
                var pivots = await _context.PersonnePivot.ToListAsync();

                ViewBag.Pivot = pivots;
                ViewBag.Secteur = secteurs;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques de la famille");
                return View();
            }
        }


    }
}
