using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class FamilleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public FamilleController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

#nullable disable
        public async Task<IActionResult> Index(int? secteur, int? pivot, string? SearchNom, string? SearchCIN)
        {
            var membres = _context.Familles
                .Include(c => c.Membres)
                .ThenInclude(c => c.Pivot)
                .Include(c => c.Membres)
                .ThenInclude(c => c.Secteur)
                .Include(c => c.Membres)
                .ThenInclude(c => c.RelationParente).AsQueryable();

            if (secteur != null && secteur != 0)
            {
                membres = membres.Where(c => c.Membres.Any(c => c.SecteurId == secteur));
            }
            if (pivot != null && pivot != 0)
            {
                membres = membres.Where(c => c.Membres.Any(c => c.PivotId == pivot));
            }
            if (!string.IsNullOrEmpty(SearchNom))
            {
                membres = membres.Where(c => c.Membres.Any(c => c.Nom.Contains(SearchNom) || c.Prenom.Contains(SearchNom)));
            }
            if (!string.IsNullOrEmpty(SearchCIN))
            {
                membres = membres.Where(c => c.Membres.Any(c => c.CIN.Contains(SearchCIN)));
            }

            var secteurs = await _context.Secteurs.ToListAsync();
            var pivots = await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync();

            ViewBag.Pivot = pivots;
            ViewBag.Secteur = new SelectList(secteurs, "Id", "Nom");

            var result = await membres.ToListAsync();

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Personnes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["FamilleId"] = new SelectList(await _context.Familles.ToListAsync(), "FamilleId", "Nom");
            var pivots = await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync();

            ViewBag.Pivot = pivots; ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        // POST: Personnes/Create
        [HttpPost]
        public async Task<IActionResult> Create(Personne personne)
        {
            try
            {
                if (personne.PivotId == null || personne.PivotId == 0)
                {
                    personne.IsResponsable = true;
                }
                else
                {
                    personne.IsResponsable = false;

                    var familleResponsable = await _context.Personnes.FirstOrDefaultAsync(c => c.PersonneId == personne.PivotId);
                    if (familleResponsable.FamilleId != null)
                    {
                        personne.FamilleId = familleResponsable.FamilleId;
                    }
                    else
                    {
                        var nouvelleFamille = new Famille { ResponsableId = familleResponsable.PersonneId };
                        await _context.AddAsync(nouvelleFamille);
                        await _context.SaveChangesAsync();

                        personne.FamilleId = nouvelleFamille.Id;
                        familleResponsable.FamilleId = nouvelleFamille.Id;
                        _context.Update(familleResponsable);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Add(personne);
                await _context.SaveChangesAsync();

                if(personne.PivotId == null || personne.PivotId == 0)
                {
                    var nouvelleFamille = new Famille { ResponsableId = personne.PersonneId };
                    await _context.AddAsync(nouvelleFamille);
                    await _context.SaveChangesAsync();

                    personne.FamilleId = nouvelleFamille.Id;
                    _context.Update(personne);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["FamilleId"] = new SelectList(await _context.Familles.ToListAsync(), "FamilleId", "Nom", personne.FamilleId);
                ViewData["PivotId"] = new SelectList(await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync(), "PersonneId", "Nom", personne.PivotId);
                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom", personne.RelationParenteId);
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom", personne.SecteurId);
                return View(personne);
            }
        }
    }
}
