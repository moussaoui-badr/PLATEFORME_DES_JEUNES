using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Election;

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
            var secteurs = await _context.Secteurs.ToListAsync();
            var pivots = await _context.Personnes.Where(c => c.PivotId == null).ToListAsync();

            if (secteur == null && pivot == null && string.IsNullOrEmpty(SearchNom) && string.IsNullOrEmpty(SearchCIN))
            {
                ViewBag.Pivot = pivots;
                ViewBag.Secteur = new SelectList(secteurs, "Id", "Nom");

                return View(new List<Personne>());
            }

            var membres = _context.Personnes
                .Include(c => c.Secteur)
                .AsQueryable();

            if (secteur != null && secteur != 0)
            {
                membres = membres.Where(c => c.SecteurId == secteur && c.PivotId == null);
            }
            if (pivot != null && pivot != 0)
            {
                membres = membres.Where(c => c.PersonneId == pivot);
            }
            if (!string.IsNullOrEmpty(SearchNom))
            {
                membres = membres.Where(c => c.Nom.Contains(SearchNom) || c.Prenom.Contains(SearchNom));
            }
            if (!string.IsNullOrEmpty(SearchCIN))
            {
                membres = membres.Where(c => c.CIN.Contains(SearchCIN));
            }

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
        public async Task<IActionResult> CreatePivot()
        {
            var pivots = await _context.Personnes.Where(c => c.PivotId == null).ToListAsync();

            ViewBag.Pivot = pivots; ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePivot(Personne personne)
        {
            try
            {
                await _context.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["PivotId"] = new SelectList(await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync(), "PersonneId", "Nom", personne.PivotId);
                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom", personne.RelationParenteId);
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom", personne.SecteurId);
                return View(personne);
            }
        }

        public async Task<IActionResult> CreateResponsable()
        {
            var pivots = await _context.Personnes.Where(c => c.PivotId == null).ToListAsync();

            ViewBag.Pivot = pivots; ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateResponsable(Personne personne)
        {
            try
            {
                await _context.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["PivotId"] = new SelectList(await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync(), "PersonneId", "Nom", personne.PivotId);
                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom", personne.RelationParenteId);
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom", personne.SecteurId);
                return View(personne);
            }
        }

        public async Task<IActionResult> CreateMember()
        {
            var pivots = await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync();

            ViewBag.Pivot = pivots; ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(Personne personne)
        {
            try
            {
                await _context.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["PivotId"] = new SelectList(await _context.Personnes.Where(c => c.IsResponsable == true).ToListAsync(), "PersonneId", "Nom", personne.PivotId);
                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom", personne.RelationParenteId);
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom", personne.SecteurId);
                return View(personne);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new PivotRespoMembreModel();

            var personne = await _context.Personnes
                .Include(c => c.Secteur)
                .Include(c => c.RelationParente)
                .Include(c => c.Pivot)
                .FirstOrDefaultAsync(m => m.PersonneId == id);

            //Vérification pivot
            if(personne.PivotId == null)
            {
                model.Pivot = personne;
                model.Responsables = await _context.Personnes.Where(c => c.PivotId == personne.PersonneId && c.IsResponsable == true).ToListAsync();
                model.Membres = await _context.Personnes.Where(c => c.PivotId == personne.PersonneId && (c.IsResponsable == null || c.IsResponsable == false)).ToListAsync();
            }
            //Vérification responsable
            else if(personne.IsResponsable == true)
            {
                model.Pivot = personne.Pivot;
                model.Responsables = await _context.Personnes.Where(c => c.PivotId == personne.PivotId && c.IsResponsable == true).ToListAsync();
                model.Membres = await _context.Personnes.Where(c => c.PivotId == personne.PivotId && (c.IsResponsable == null || c.IsResponsable == false)).ToListAsync();
            }
            //Vérification membre
            else if(personne.PivotId != null && (personne.IsResponsable == null || personne.IsResponsable == false))
            {
                model.Pivot = personne.Pivot;
                model.Responsables = await _context.Personnes.Where(c => c.PivotId == personne.PivotId && c.IsResponsable == true).ToListAsync();
                model.Membres = await _context.Personnes.Where(c => c.PivotId == personne.PivotId && (c.IsResponsable == null || c.IsResponsable == false)).ToListAsync();
            }

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
