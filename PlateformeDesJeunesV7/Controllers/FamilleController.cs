using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Election;
using Domain.Entities.V2;
using Repository.Data.Migrations;

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
            try
            {
                var datenow = DateTime.Now.Date;
                var startDate = new DateTime(datenow.Year, 5, 20);
                var endDate = new DateTime(datenow.Year, 5, 25);

                var anniversaireProche = await _context.PersonnePivot
                    .Where(c => c.DateNaissance != null &&
                                c.DateNaissance.Value.Month == 5 &&
                                c.DateNaissance.Value.Day >= 20 &&
                                c.DateNaissance.Value.Day <= 25)
                    .Select(c => new PersonnePivot
                    {
                        Nom = c.Nom + " " + c.Prenom,
                        DateNaissance = c.DateNaissance,
                        GSM = c.GSM,
                        Adresse = c.Adresse,
                        Secteur = c.Secteur,
                        DateCreation = c.DateCreation
                    })
                    .ToListAsync();

                var anniversaireProcheResponsables = await _context.PersonneResponsable
                    .Where(c => c.DateNaissance != null &&
                                c.DateNaissance.Value.Month == 5 &&
                                c.DateNaissance.Value.Day >= 20 &&
                                c.DateNaissance.Value.Day <= 25)
                    .Select(c => new PersonnePivot
                    {
                        Nom = c.Nom + " " + c.Prenom,
                        DateNaissance = c.DateNaissance,
                        GSM = c.GSM,
                        Adresse = c.Adresse,
                        Secteur = c.Secteur,
                        DateCreation = c.DateCreation
                    })
                    .ToListAsync();

                var anniversaireProcheMembres = await _context.PersonneMembre
                    .Where(c => c.DateNaissance != null &&
                                c.DateNaissance.Value.Month == 5 &&
                                c.DateNaissance.Value.Day >= 20 &&
                                c.DateNaissance.Value.Day <= 25)
                    .Select(c => new PersonnePivot
                    {
                        Nom = c.Nom + " " + c.Prenom,
                        DateNaissance = c.DateNaissance,
                        GSM = c.GSM,
                        Adresse = c.Adresse,
                        Secteur = c.Secteur,
                        DateCreation = c.DateCreation
                    })
                    .ToListAsync();

                var allAnniversairesProche = anniversaireProche.Concat(anniversaireProcheResponsables)
                                                              .Concat(anniversaireProcheMembres)
                                                              .ToList();

                ViewBag.AnniversaireProche = allAnniversairesProche;

            }
            catch (Exception ex)
            {
                
            }

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
                membres = membres.Where(c => c.PersonnePivotId == model.PivotId || c.Responsables.Any(a => a.PivotId == model.PivotId) || c.Responsables.Any(b => b.Pivot.PersonnePivotId == model.PivotId));
            }
            if (model.SecteurId != null && model.SecteurId != 0)
            {
                membres = membres.Where(c => c.SecteurId == model.SecteurId || c.Responsables.Any(a => a.SecteurId == model.SecteurId) || c.Responsables.Any(b => b.Pivot.SecteurId == model.SecteurId));
            }
            if (!string.IsNullOrEmpty(model.SearchNom))
            {
                membres = membres.Where(c => c.Nom.Contains(model.SearchNom) || c.Prenom.Contains(model.SearchNom) || c.Responsables.Any(a => a.Nom == model.SearchNom || a.Prenom == model.SearchNom) || c.Responsables.Any(b => b.Nom == model.SearchNom || b.Prenom == model.SearchNom));
            }
            if (!string.IsNullOrEmpty(model.SearchCIN))
            {
                membres = membres.Where(c => c.CIN.Contains(model.SearchCIN) || c.Responsables.Any(a => a.CIN == model.SearchCIN) || c.Responsables.Any(b => b.Pivot.CIN == model.SearchCIN));
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
                var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
                if(CINExist == true)
                {
                    ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
                    ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                    return View(personne);
                }
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
                var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
                if (CINExist == true)
                {
                    var pivots = await _context.PersonnePivot.ToListAsync();
                    ViewBag.Pivot = pivots;
                    ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
                    ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

                    ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                    return View(personne);
                }
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
                var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
                if (CINExist == true)
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


                    ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                    return View(personne);
                }
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
            try 
            {
                var CINExist = await _context.PersonnePivot.Where(c => c.PersonnePivotId != id && c.CIN == personne.CIN).CountAsync() > 1 || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
                if (CINExist == true)
                {
                    ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
                    ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                    return View(personne);
                }
                var pivot = await _context.PersonnePivot.FindAsync(id);
                pivot.Nom = personne.Nom;
                pivot.Prenom = personne.Prenom;
                pivot.CIN = personne.CIN;
                pivot.DateNaissance = personne.DateNaissance;
                pivot.GSM = personne.GSM;
                pivot.Adresse = personne.Adresse;
                pivot.SecteurId = personne.SecteurId;

                _context.Update(pivot);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");
                return View(personne);
            }
        }
        //Edit Responsable
        public async Task<IActionResult> EditResponsable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pivots = await _context.PersonnePivot.ToListAsync();
            ViewBag.Pivot = pivots;
            ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

            var pivotsSelect = new SelectList(pivots?.Select(c => new Domain.Entities.V2.PersonnePivot
            {
                PersonnePivotId = c.PersonnePivotId,
                Nom = c.Nom + " " + c.Prenom
            }).ToList(), "PersonnePivotId", "Nom") ?? new SelectList(new List<Domain.Entities.V2.PersonnePivot>());
            ViewBag.Pivot = pivotsSelect;

            var personne = await _context.PersonneResponsable.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // POST: Personnes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResponsable(int id, PersonneResponsable personne)
        {
            var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.Where(c => c.PersonneResponsableId != id && c.CIN == personne.CIN).CountAsync() > 1 || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
            if (CINExist == true)
            {
                var pivots = await _context.PersonnePivot.ToListAsync();
                ViewBag.Pivot = pivots;
                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

                var pivotsSelect = new SelectList(pivots?.Select(c => new Domain.Entities.V2.PersonnePivot
                {
                    PersonnePivotId = c.PersonnePivotId,
                    Nom = c.Nom + " " + c.Prenom
                }).ToList(), "PersonnePivotId", "Nom") ?? new SelectList(new List<Domain.Entities.V2.PersonnePivot>());
                ViewBag.Pivot = pivotsSelect;

                ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                return View(personne);
            }

            var responsable = await _context.PersonneResponsable.FindAsync(id);
            responsable.Nom = personne.Nom;
            responsable.Prenom = personne.Prenom;
            responsable.CIN = personne.CIN;
            responsable.DateNaissance = personne.DateNaissance;
            responsable.GSM = personne.GSM;
            responsable.Adresse = personne.Adresse;
            responsable.SecteurId = personne.SecteurId;
            responsable.PivotId = personne.PivotId;

            _context.Update(responsable);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Edit Membre
        public async Task<IActionResult> EditMembre(int? id)
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

            ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

            var pivotsSelect = new SelectList(pivots?.Select(c => new Domain.Entities.V2.PersonnePivot
            {
                PersonnePivotId = c.PersonnePivotId,
                Nom = c.Nom + " " + c.Prenom
            }).ToList(), "PersonnePivotId", "Nom") ?? new SelectList(new List<Domain.Entities.V2.PersonnePivot>());

            ViewBag.Pivot = pivotsSelect;

            ViewBag.ResponsableSelect = new SelectList(responsables, "PersonneResponsableId", "Nom");

            return View(personne);
        }

        // POST: Personnes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMembre(int id, PersonneMembre personne)
        {
            var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.Where(c => c.PersonneMembreId != id && c.CIN == personne.CIN ).CountAsync() > 1;
            if (CINExist == true)
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

                ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
                ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

                var pivotsSelect = new SelectList(pivots?.Select(c => new Domain.Entities.V2.PersonnePivot
                {
                    PersonnePivotId = c.PersonnePivotId,
                    Nom = c.Nom + " " + c.Prenom
                }).ToList(), "PersonnePivotId", "Nom") ?? new SelectList(new List<Domain.Entities.V2.PersonnePivot>());

                ViewBag.Pivot = pivotsSelect;

                ViewBag.ResponsableSelect = new SelectList(responsables, "PersonneResponsableId", "Nom");

                ModelState.AddModelError("CIN", "Ce CIN existe déjà");

                return View(personne);
            }

            var personneMembre = await _context.PersonneMembre.FindAsync(id);
            personneMembre.Nom = personne.Nom;
            personneMembre.Prenom = personne.Prenom;
            personneMembre.CIN = personne.CIN;
            personneMembre.DateNaissance = personne.DateNaissance;
            personneMembre.GSM = personne.GSM;
            personneMembre.Adresse = personne.Adresse;
            personneMembre.SecteurId = personne.SecteurId;
            personneMembre.PivotId = personne.PivotId;
            personneMembre.ResponsableId = personne.ResponsableId;

            //Update
            _context.Update(personneMembre);
            await _context.SaveChangesAsync();
                
            return RedirectToAction(nameof(Index));
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
        //Delete Responsable
        public async Task<IActionResult> DeleteResponsable(int? id)
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
            else
            {
                _context.PersonneResponsable.Remove(personne);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        //Delete Pivot
        public async Task<IActionResult> DeletePivot(int? id)
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
            else
            {
                _context.PersonnePivot.Remove(personne);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        
    }
}
