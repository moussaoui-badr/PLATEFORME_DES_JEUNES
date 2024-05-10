using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Election;
using Domain.Entities.V2;

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
        public async Task<IActionResult> Index(StatistiqueFamille model)
        {
            var secteurs = await _context.Secteurs.ToListAsync();
            var pivots = await _context.PersonnePivot.ToListAsync();

            ViewBag.Pivot = pivots;
            ViewBag.Secteur = secteurs;

            if (model.SecteurId == null && model.PivotId == null && string.IsNullOrEmpty(model.SearchNom) && string.IsNullOrEmpty(model.SearchCIN))
            {
                return View(model);
            }

            var membres = _context.PersonnePivot
                .Include(c => c.Secteur)
                .Include(c => c.Responsables)
                .ThenInclude(c => c.Membres)
                .ThenInclude(c => c.RelationParente)
                .AsQueryable();

            //Récupération du pivot et de ses responsables et membres
            if (model.PivotId != null && model.PivotId != 0)
            {
                membres = membres.Where(c => c.PersonnePivotId == model.PivotId);
            }
            if (model.SecteurId != null && model.SecteurId != 0)
            {
                membres = membres.Where(c => c.SecteurId == model.SecteurId );
            }
            if (!string.IsNullOrEmpty(model.SearchNom))
            {
                membres = membres.Where(c => c.Nom.Contains(model.SearchNom) || c.Prenom.Contains(model.SearchNom) );
            }
            if (!string.IsNullOrEmpty(model.SearchCIN))
            {
                membres = membres.Where(c => c.CIN.Contains(model.SearchCIN));
            }

            var result = await membres.ToListAsync();
            model.PersonnePivot = result;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Personnes/Create
        public async Task<IActionResult> CreatePivot()
        {
            var pivots = await _context.PersonnePivot.ToListAsync();

            ViewBag.Pivot = pivots; ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePivot(PersonnePivot personne)
        {
            try
            {
                await _context.PersonnePivot.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<IActionResult> CreateResponsable()
        {
            var pivots = await _context.PersonnePivot.ToListAsync();

            ViewBag.Pivot = pivots; 
            ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateResponsable(PersonneResponsable personne)
        {
            try
            {
                await _context.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<IActionResult> CreateMember()
        {
            var pivots = await _context.PersonnePivot.ToListAsync();
            var responsables = await _context.PersonneResponsable.Select(c => new PersonneResponsable
            {
                PersonneResponsableId = c.PersonneResponsableId,
                Nom = c.Nom + " " + c.Prenom,
                PivotId = c.PivotId
            }).ToListAsync();

            //Convertir en json responsables
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(responsables);

            ViewBag.Responsable = json;
            ViewBag.Pivot = pivots;

            ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(PersonneMembre personne)
        {
            try
            {
                await _context.AddAsync(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        

        //Mofification PersonnePivot
        public async Task<IActionResult> EditPivot(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.PersonnePivot.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }
        // POST: Personnes/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPivot(int id, PersonnePivot personne)
        {
            if (id != personne.PersonnePivotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(personne);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }
        //Edit Responsable
        public async Task<IActionResult> EditResponsable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.PersonneResponsable.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }

        // POST: Personnes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResponsable(int id, PersonneResponsable personne)
        {
            if (id != personne.PersonneResponsableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(personne);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }

        //Edit Membre
        public async Task<IActionResult> EditMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.PersonneMembre.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }

        // POST: Personnes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(int id, PersonneMembre personne)
        {
            if (id != personne.PersonneMembreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(personne);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
            return View(personne);
        }

        //Delete Membre
        public async Task<IActionResult> DeleteMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.PersonneMembre.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }
            else
            {
                _context.PersonneMembre.Remove(personne);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        
    }
}
