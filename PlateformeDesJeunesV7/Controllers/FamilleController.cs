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
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin, GestionnaireFamille")]
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
                var nextMonth = datenow.AddMonths(1);
                var startDay = datenow.Day;
                var endDay = datenow.Day;

                var anniversaireProche = await _context.PersonnePivot
                    .Where(c => c.DateNaissance != null &&
                                ((c.DateNaissance.Value.Month == nextMonth.Month && c.DateNaissance.Value.Day >= startDay) ||
                                 (c.DateNaissance.Value.Month == nextMonth.AddMonths(1).Month && c.DateNaissance.Value.Day <= endDay)))
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
                                ((c.DateNaissance.Value.Month == nextMonth.Month && c.DateNaissance.Value.Day >= startDay) ||
                                 (c.DateNaissance.Value.Month == nextMonth.AddMonths(1).Month && c.DateNaissance.Value.Day <= endDay)))
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
                                ((c.DateNaissance.Value.Month == nextMonth.Month && c.DateNaissance.Value.Day >= startDay) ||
                                 (c.DateNaissance.Value.Month == nextMonth.AddMonths(1).Month && c.DateNaissance.Value.Day <= endDay)))
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

            if (model.SecteurId == null && model.PivotId == null && model.ResponsableId == null && model.MembreId == null && string.IsNullOrEmpty(model.SearchNom) && string.IsNullOrEmpty(model.SearchCIN) && (model?.TotalGeneral == null || model?.TotalGeneral == 0))
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
            if(model.ResponsableId != null && model.ResponsableId != 0)
            {
                membres = membres.Where(c => c.Responsables.Any(r => r.PersonneResponsableId == model.ResponsableId));
            }
            if (model.MembreId != null && model.MembreId != 0)
            {
                membres = membres.Where(c => c.Responsables.Any(m => m.Membres.Any(m => m.PersonneMembreId == model.MembreId)));
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
                if (personne.PivotId == null || personne.PivotId == 0)
                {
                    ModelState.AddModelError("PivotId", "Veuiller selectionner un pivot"); 
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
                //var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.AnyAsync(c => c.CIN == personne.CIN);
                //if (CINExist == true)
                //{
                //    var pivots = await _context.PersonnePivot.ToListAsync();
                //    var responsables = await _context.PersonneResponsable.Select(c => new PersonneResponsable
                //    {
                //        PersonneResponsableId = c.PersonneResponsableId,
                //        Nom = c.Nom + " " + c.Prenom,
                //        PivotId = c.PivotId
                //    }).ToListAsync();

                //    //Convertir en json responsables
                //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(responsables);

                //    ViewBag.Responsable = json;
                //    ViewBag.Pivot = pivots;

                //    ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
                //    ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");


                //    ModelState.AddModelError("CIN", "Ce CIN existe déjà");
                //    return View(personne);
                //}

                if (personne.ResponsableId == null || personne.ResponsableId == 0)
                {
                    ModelState.AddModelError("ResponsableId", "Veuiller selectionner un responsable");
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
            //var CINExist = await _context.PersonnePivot.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneResponsable.AnyAsync(c => c.CIN == personne.CIN) || await _context.PersonneMembre.Where(c => c.PersonneMembreId != id && c.CIN == personne.CIN ).CountAsync() > 1;
            //if (CINExist == true)
            //{
            //    var pivots = await _context.PersonnePivot.ToListAsync();
            //    var responsables = await _context.PersonneResponsable.Select(c => new PersonneResponsable
            //    {
            //        PersonneResponsableId = c.PersonneResponsableId,
            //        Nom = c.Nom + " " + c.Prenom,
            //        PivotId = c.PivotId
            //    }).ToListAsync();

            //    //Convertir en json responsables
            //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(responsables);

            //    ViewBag.Responsable = json;

            //    ViewData["RelationParenteId"] = new SelectList(await _context.TypesRelationParente.ToListAsync(), "Id", "Nom");
            //    ViewData["SecteurId"] = new SelectList(await _context.Secteurs.ToListAsync(), "Id", "Nom");

            //    var pivotsSelect = new SelectList(pivots?.Select(c => new Domain.Entities.V2.PersonnePivot
            //    {
            //        PersonnePivotId = c.PersonnePivotId,
            //        Nom = c.Nom + " " + c.Prenom
            //    }).ToList(), "PersonnePivotId", "Nom") ?? new SelectList(new List<Domain.Entities.V2.PersonnePivot>());

            //    ViewBag.Pivot = pivotsSelect;

            //    ViewBag.ResponsableSelect = new SelectList(responsables, "PersonneResponsableId", "Nom");

            //    ModelState.AddModelError("CIN", "Ce CIN existe déjà");

            //    return View(personne);
            //}

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


        public async Task<IActionResult> GeneratePivotExcel()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ExcelCandidats", "MaListe.xlsx");

            // Remplacer cette ligne par la méthode appropriée pour obtenir vos pivots
            var pivots = await _context.PersonnePivot
                .Include(c => c.Secteur)
                .Include(p => p.Responsables)
                .ThenInclude(c => c.Secteur)
                .Include(p => p.Responsables)
                .ThenInclude(c => c.Secteur)
                .Include(p => p.Responsables)
                .ThenInclude(r => r.Membres)

                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Pivots");

            // Ajouter les en-têtes
            worksheet.Cells[1, 1].Value = "Nom";
            worksheet.Cells[1, 2].Value = "Prénom";
            worksheet.Cells[1, 3].Value = "CIN";
            worksheet.Cells[1, 4].Value = "Date de naissance";
            worksheet.Cells[1, 5].Value = "Adresse";
            worksheet.Cells[1, 6].Value = "GSM";
            worksheet.Cells[1, 7].Value = "Date de création";
            worksheet.Cells[1, 8].Value = "Secteur";

            int rowIndex = 2;
            foreach (var pivot in pivots)
            {
                // Ajouter les informations du pivot
                worksheet.Cells[rowIndex, 1].Value = pivot.Nom;
                worksheet.Cells[rowIndex, 2].Value = pivot.Prenom;
                worksheet.Cells[rowIndex, 3].Value = pivot.CIN;
                worksheet.Cells[rowIndex, 4].Value = pivot.DateNaissance?.ToString("yyyy-MM-dd");
                worksheet.Cells[rowIndex, 5].Value = pivot.Adresse;
                worksheet.Cells[rowIndex, 6].Value = pivot.GSM;
                worksheet.Cells[rowIndex, 7].Value = pivot.DateCreation.ToString("yyyy-MM-dd");
                worksheet.Cells[rowIndex, 8].Value = pivot.Secteur?.Nom; // Suppose que Secteur a une propriété Nom

                // Appliquer le style rouge aux lignes des pivots
                using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 8])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Red);
                }

                rowIndex++;

                // Ajouter les responsables pour ce pivot
                if (pivot.Responsables != null)
                {
                    foreach (var responsable in pivot.Responsables)
                    {
                        // Ajouter les informations du responsable
                        worksheet.Cells[rowIndex, 1].Value = responsable.Nom;
                        worksheet.Cells[rowIndex, 2].Value = responsable.Prenom;
                        worksheet.Cells[rowIndex, 3].Value = responsable.CIN;
                        worksheet.Cells[rowIndex, 4].Value = responsable.DateNaissance?.ToString("yyyy-MM-dd");
                        worksheet.Cells[rowIndex, 5].Value = responsable.Adresse;
                        worksheet.Cells[rowIndex, 6].Value = responsable.GSM;
                        worksheet.Cells[rowIndex, 7].Value = responsable.DateCreation.ToString("yyyy-MM-dd");
                        worksheet.Cells[rowIndex, 8].Value = responsable.Secteur?.Nom;

                        // Appliquer le style jaune aux lignes des responsables
                        using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 8])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }

                        rowIndex++;

                        // Ajouter les membres pour ce responsable
                        if (responsable.Membres != null)
                        {
                            foreach (var membre in responsable.Membres)
                            {
                                // Ajouter les informations du membre
                                worksheet.Cells[rowIndex, 1].Value = membre.Nom;
                                worksheet.Cells[rowIndex, 2].Value = membre.Prenom;
                                worksheet.Cells[rowIndex, 3].Value = membre.CIN;
                                worksheet.Cells[rowIndex, 4].Value = membre.DateNaissance?.ToString("yyyy-MM-dd");
                                worksheet.Cells[rowIndex, 5].Value = membre.Adresse;
                                worksheet.Cells[rowIndex, 6].Value = membre.GSM;
                                worksheet.Cells[rowIndex, 7].Value = membre.DateCreation.ToString("yyyy-MM-dd");
                                worksheet.Cells[rowIndex, 8].Value = membre.Secteur?.Nom;

                                // Appliquer le style blanc aux lignes des membres
                                using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 8])
                                {
                                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                                }

                                rowIndex++;
                            }
                        }
                    }
                }
            }

            // Mise en forme des en-têtes
            using (var range = worksheet.Cells[1, 1, 1, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            // Ajuster automatiquement la largeur des colonnes
            worksheet.Cells.AutoFitColumns();

            // Sauvegarder le fichier Excel
            string excelName = $"ListePivots.xlsx";
            return File(package.GetAsByteArray(), "application/octet-stream", excelName);
        }

    }
}
