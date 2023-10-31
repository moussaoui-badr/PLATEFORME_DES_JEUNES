using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa.AspNetCore;
using Service.IService;
using System.Drawing;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace PlateformeDesJeunesAinSebaa.Controllers
{
#pragma warning disable
#nullable disable
    [Authorize(Roles = "Admin, Finance")]
    public class FinancesController : Controller
    {
        private readonly ICandidatINDHService _candidatService;
        private readonly IEmailService _emailService;

        public FinancesController(ICandidatINDHService candidatService, IEmailService emailService)
        {
            _candidatService = candidatService;
            _emailService = emailService;
        }
        //public IActionResult Index()
        //{

        //    return View();

        //}
        public async Task<IActionResult> MonModel(int id)
        {
            var client = await _candidatService.GetCandidat(id);

            return View("~/Views/Finances/MonModel.cshtml", client);
        }

        public async Task<IActionResult> PrintPdf(int id)
        {
            var client = await _candidatService.GetCandidat(id); // Assume synchronous method
            var pdfResult = await new ViewAsPdf("~/Views/Finances/MonModel.cshtml", client).BuildFile(ControllerContext);

            // Return PDF as downloadable file
            return File(pdfResult, "application/pdf", client.Nom + "_" + DateTime.Now.ToString("dd_MM_yy") + ".pdf");
        }


        public async Task<IActionResult> Filtre(int? EtatMateriel, int? ApportPersonnel)
        {
            try
            {
                ViewData["EtatMateriel"] = EtatMateriel;
                ViewData["ApportPersonnel"] = ApportPersonnel;

                return View(await _candidatService.FiltreMaterielIndh(EtatMateriel, ApportPersonnel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Filtre", "Finances", new { message = ex.Message });
            }
        }
        public async Task<IActionResult> Index(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string PGFilter)
        {
            try
            {
                ViewData["CINSortParm"] = SearchCIN;
                ViewData["CurrentFilterN"] = SearchNom;
                ViewData["CurrentFilterP"] = SearchPrenom;
                ViewData["CurrentFilterD"] = SearchDate;
                ViewData["CurrentFilterDO"] = SearchDateO;
                ViewData["PlateformeGestionnaireSortParm"] = PGFilter;
                return View(await _candidatService.GetCandidats(PGFilter, SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, 15));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Finances", new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateClientINDH model)
        {
            try
            {
                //if(model.Candidat.MontantProjet != (model.Candidat.MontantApportPersonnel + model.Candidat.MontantINDH))
                //{
                //    ViewData["INDHError"] = "Le montant du projet n'est pas égal a la somme du montant apport personnel et le montant INDH.";
                //    return View(model);
                //}

                var response = await _candidatService.AddCandidat(model.Candidat);
                if (model.DocumentModel != null)
                {
                    var responseDocument = await _candidatService.AddDocument(model.DocumentModel, response.ID);
                }
                if (model.INDHs != null)
                {
                    var responseINDH = await _candidatService.AddIndh(model.INDHs, response.ID, model.Candidat.PlateformeGestionnaire);
                }
                return RedirectToAction("Index", "Finances", new { Message = "Success" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ID)
        {
            try
            {
                var candidat = await _candidatService.GetCandidatEdit(ID);
                return View(candidat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Edit", "Finances", new { message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateClientINDH model)
        {
            try
            {
                //if (model.Candidat.MontantProjet != (model.Candidat.MontantApportPersonnel + model.Candidat.MontantINDH))
                //{
                //    ViewData["INDHError"] = "Le montant du projet n'est pas égal a la somme du montant apport personnel et le montant INDH";
                //    return View(model);
                //}

                var response = await _candidatService.EditCandidat(new ClientFinance
                {
                    ClientFinanceID = model.Candidat.ClientID,
                    Nom = model.Candidat.Nom,
                    Prenom = model.Candidat.Prenom,
                    Adresse = model.Candidat.Adresse,
                    DateNaissance = model.Candidat.DateNaissance,
                    Sexe = model.Candidat.Sexe,
                    Email = model.Candidat.Email,
                    Telephone = model.Candidat.Telephone,
                    CIN = model.Candidat.CIN,
                    DateAderation = model.Candidat.DateAderation,
                    Created = DateTime.Now,
                    MontantApportPersonnel = model.Candidat.MontantApportPersonnel,
                    MontantINDH = model.Candidat.MontantINDH,
                    MontantProjet = model.Candidat.MontantProjet,
                    PlateformeGestionnaire = model.Candidat.PlateformeGestionnaire,
                    Commentaire = model.Candidat.Commentaire,
                    File = model.Candidat.ImageUrl,
                    ImageUrl = model.Candidat.ImageUrlString,
                    ApportPersonnelConfirme = model.Candidat.ApportPersonnelConfirme
                });

                if (model.DocumentModel != null)
                {
                    await _candidatService.EditDocument(model.DocumentModel);
                }

                if (model.INDHs != null)
                {
                    await _candidatService.EditIndh(model.INDHs, model.Candidat.PlateformeGestionnaire);
                }


                return RedirectToAction("Index", "Finances");

            }
            catch (Exception ex)
            {
                ViewData["MessageDocument"] = ex.Message.ToString();
                Console.WriteLine(ex.Message.ToString());
                ViewData["Message"] = ex.Message.ToString();
                Console.WriteLine(ex.Message.ToString());
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int ID)
        {
            return View(await _candidatService.GetCandidat(ID));
        }

        //public async Task<IActionResult> PrintPdf(int id)
        //{
        //    try
        //    {
        //        var stream = await _candidatService.PrintPdf(id);

        //        // Générer un nom de fichier unique
        //        var fileName = $"Candidat_{id}.pdf";

        //        // Retourner le PDF généré en tant que fichier à télécharger
        //        return File(stream, "application/pdf", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index", new { ex.Message });
        //    }

        //}


        [HttpGet]
        public async Task<IActionResult> Delete(int id, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN)
        {
            var result = await _candidatService.Delete(id);
            if (result.Success) return RedirectToAction("Index", new
            {
                SearchNom,
                SearchPrenom,
                SearchDate,
                SearchCIN,
                SearchDateO,
                pageNumber
            });
            else return RedirectToAction("Index", new { Error = "ErreurSuppression" });
        }


        public async Task<IActionResult> SaisieBGExcel(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string PGFilter)
        {
            var candidats = await _candidatService.GetCandidats(PGFilter, SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, 500);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                // Add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
                //Add the headers
                worksheet.Cells[1, 1].Value = "Nom et prénom";
                worksheet.Cells[1, 2].Value = "Date d'adèration";
                worksheet.Cells[1, 3].Value = "Numéro de téléphone";
                worksheet.Cells[1, 4].Value = "Sexe";
                worksheet.Cells[1, 5].Value = "Adresse";
                worksheet.Cells[1, 6].Value = "Email";
                worksheet.Cells[1, 7].Value = "CIN";
                worksheet.Cells[1, 8].Value = "Date de naissance";
                worksheet.Cells[1, 9].Value = "Commentaire ";
                worksheet.Cells[1, 10].Value = "Plateforme Gestionnaire ";
                worksheet.Cells[1, 11].Value = "Montant Projet ";
                worksheet.Cells[1, 12].Value = "Montant Apport Personnel ";
                worksheet.Cells[1, 13].Value = "Montant INDH ";
                //Add some items...
                for (int i = 0; i < candidats.Count; i++)
                {
                    worksheet.Cells["A" + (i + 2)].Value = candidats[i].Nom + ", " + candidats[i].Prenom;
                    worksheet.Cells["B" + (i + 2)].Value = candidats[i].DateAderation.HasValue == true ? candidats[i].DateAderation.Value.ToShortDateString() : "";
                    worksheet.Cells["C" + (i + 2)].Value = candidats[i].Telephone;
                    worksheet.Cells["D" + (i + 2)].Value = candidats[i].Sexe;
                    worksheet.Cells["E" + (i + 2)].Value = candidats[i].Adresse;
                    worksheet.Cells["F" + (i + 2)].Value = candidats[i].Email;
                    worksheet.Cells["G" + (i + 2)].Value = candidats[i].CIN;
                    worksheet.Cells["H" + (i + 2)].Value = candidats[i].DateNaissance;
                    worksheet.Cells["I" + (i + 2)].Value = candidats[i].Commentaire;
                    worksheet.Cells["J" + (i + 2)].Value = candidats[i].PlateformeGestionnaire.ToString();
                    worksheet.Cells["K" + (i + 2)].Value = candidats[i].MontantProjet;
                    worksheet.Cells["L" + (i + 2)].Value = candidats[i].MontantApportPersonnel;
                    worksheet.Cells["M" + (i + 2)].Value = candidats[i].MontantINDH;
                }

                //Ok now format the values;
                using (var range = worksheet.Cells[1, 1, 1, 13])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }
                worksheet.Cells["A1:M" + candidats.Count + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:M" + candidats.Count + 1].Style.Font.Bold = true;
                //worksheet.Cells["A6:E6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //worksheet.Cells["A6:E6"].Style.Font.Bold = true;

                //Create an autofilter for the range
                worksheet.Cells["A1:M" + candidats.Count + 1].AutoFilter = true;

                worksheet.Cells["A1:M" + candidats.Count + 1].Style.Numberformat.Format = "@";   //Format as text

                //There is actually no need to calculate, Excel will do it for you, but in some cases it might be useful. 
                //For example if you link to this workbook from another workbook or you will open the workbook in a program that hasn't a calculation engine or 
                //you want to use the result of a formula in your program.
                worksheet.Calculate();

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // lets set the header text 
                worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
                // add the page number to the footer plus the total number of pages
                worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                // add the sheet name to the footer
                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                // add the file path to the footer
                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
                worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:M"];

                // set some document properties
                package.Workbook.Properties.Title = "Liste des candidats";
                package.Workbook.Properties.Author = "Moussaoui badr-eddine";
                package.Workbook.Properties.Comments = "Candidats";

                // set some extended property values
                package.Workbook.Properties.Company = "B.Moussaoui";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "B.M");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "B.M");
                //var xlFile = new System.IO.FileInfo("sample1AA1.xlsx");
                // save our new workbook in the output directory and we are done!
                //package.SaveAs(xlFile);
                await package.SaveAsync();

                stream.Position = 0;
                string excelName = $"CandidatsPlateformeDesJeunes.xlsx";
                return File(stream, "application/octet-stream", excelName);
            }
        }

        [HttpGet]
        public async Task DeleteIndh(int ID)
        {
            try
            {
                await _candidatService.DeleteIndh(ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        public IActionResult DownloadFile(string filename)
        {
            var memory = DownloadSinghFile(filename, "Canevas");
            return File(memory.ToArray(), "application/vnd.ms-excel", filename);
        }

        private static MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }


    }
}
