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
                var pivotParSecteurQuery = _context.PersonnePivot.Include(c => c.Responsables).AsQueryable();
                var responsableParSecteurQuery = _context.PersonneResponsable.Include(c => c.Pivot).Include(c => c.Membres).AsQueryable();
                var membreParSecteurQuery = _context.PersonneMembre.Include(c => c.Responsable).ThenInclude(c => c.Pivot).AsQueryable();

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

                if (model?.ResponsableId != null)
                {
                    responsableParSecteurQuery = responsableParSecteurQuery.Where(p => p.PersonneResponsableId == model.ResponsableId);
                    membreParSecteurQuery = membreParSecteurQuery.Where(p => p.ResponsableId == model.ResponsableId);
                }

                if (model?.MembreId != null)
                {
                    membreParSecteurQuery = membreParSecteurQuery.Where(p => p.PersonneMembreId == model.MembreId);
                }

                model.NombrePivot = model?.MembreId != null ? await membreParSecteurQuery.Select(c => c.Responsable).Select(c => c.Pivot).CountAsync() : model.ResponsableId != null ? await responsableParSecteurQuery.Select(c => c.Pivot).CountAsync()  : await pivotParSecteurQuery.CountAsync();
                model.NombreResponsable = model?.MembreId != null ? await membreParSecteurQuery.Select(c => c.Responsable).CountAsync() : await responsableParSecteurQuery.CountAsync();
                model.NombreMembre = await membreParSecteurQuery.CountAsync();

                model.TotalGeneral = model.NombrePivot + model.NombreResponsable + model.NombreMembre;

                var secteurs = await _context.Secteurs.ToListAsync();

                var pivots = await _context.PersonnePivot.ToListAsync();
                var responsables = await _context.PersonneResponsable.ToListAsync();
                var membres = await _context.PersonneMembre.ToListAsync();

                ViewBag.Secteur = secteurs;

                ViewBag.Pivot = pivots;
                ViewBag.Responsable = responsables;
                ViewBag.Membre = membres;

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
