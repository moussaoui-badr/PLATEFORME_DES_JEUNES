using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa.AspNetCore;
using Service.IService;
using Service.StaticHelperService;
using System.Diagnostics;
using System.Drawing;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace PlateformeDesJeunesAinSebaa.Controllers
{
#pragma warning disable
#nullable disable
    [Authorize(Roles = "Admin, Gestionnaire, HeureJoyeuse, Amideast")]
    public class CandidatsController : Controller
    {
        private readonly ICandidatService _candidatService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _hosting;
        private readonly IConverter _pdfConverter;
        private readonly IConfiguration _configuration;
        //var baseUrl = configuration["Urls:" + env.EnvironmentName];

        public CandidatsController(ICandidatService candidatService, IEmailService emailService, IWebHostEnvironment hosting, IConverter pdfConverter)
        {
            _candidatService = candidatService;
            _emailService = emailService;
            _hosting = hosting;
            _pdfConverter = pdfConverter;
        }

        public IActionResult Exception()
        {
            try
            {
                var exceptionMessage = TempData["ExceptionMessage"] as string;
                if (!string.IsNullOrEmpty(exceptionMessage))
                {
                    // Use the exceptionMessage to display error message or handle the exception as required
                    ViewBag.ExceptionMessage = exceptionMessage;
                    // Clear the TempData to ensure it's not available after this request
                    TempData.Clear();
                }
                //await _emailSenderService.SendEmailAsync("b.moussaoui@Alexsys.solutions", "Exception", exceptionMessage ?? "");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> MonModel(int id)
        {
            var client = await _candidatService.GetCandidat(id);

            return View("~/Views/Candidats/MonModel.cshtml", client);
        }


        public async Task<IActionResult> PrintPdf(int id)
        {
            var client = await _candidatService.GetCandidat(id); // Assume synchronous method
            var pdfResult = await new ViewAsPdf("~/Views/Candidats/MonModel.cshtml", client).BuildFile(ControllerContext);

            // Return PDF as downloadable file
            return File(pdfResult, "application/pdf", client.Nom + "_" + DateTime.Now.ToString("dd_MM_yy") + ".pdf");
        }

        public async Task<IActionResult> GeneratePDF(int id)
        {
            var client = await _candidatService.GetCandidat(id);

            string uploads = Path.Combine(_hosting.WebRootPath, "Templates");
            string templatePath = Path.Combine(uploads, "EmailTemplate2.html");

            var templateContent = System.IO.File.ReadAllText(templatePath);


            string uploadsImage = Path.Combine(_hosting.WebRootPath, "Uploads");
            string fullPathImage = Path.Combine(uploadsImage, client.ImageUrl);
            // Remplacez les placeholders par les données réelles
            templateContent = templateContent.Replace("[Nom]", client.Nom)
            .Replace("[Prenom]", client.Prenom)
            .Replace("[Adresse]", client.Adresse)
                .Replace("[DateNaissance]", client.DateNaissance.GetValueOrDefault().ToString("yyyy-MM-dd"))
            .Replace("[Cin]", client.CIN)
            .Replace("[ImageUrl]", fullPathImage)
                .Replace("[DecouvertePlateforme]", client.DecouvertePlateForme)
                .Replace("[DateAderation]", client.DateAderation?.ToString("yyyy-MM-dd"))
            .Replace("[Email]", client.Email)
                .Replace("[Sexe]", client.Sexe.ToString())
                .Replace("[Oriente]", client.Oriente.GetValueOrDefault() ? "Oui" : "Non");

            HtmlToPdfDocument pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = DinkToPdf.Orientation.Portrait,
            },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = templateContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
            };

            byte[] pdfBytes = _pdfConverter.Convert(pdf);

            return File(pdfBytes, "application/pdf", "example.pdf");
        }

        //Download File
        public async Task<IActionResult> DownloadFile(string NameFolder, string fileName)
        {
            var stream = await CopyFile.GetFileStreamAsync(_hosting, NameFolder, fileName);
            return File(stream, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> SaisieBGExcel(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ExcelCandidats", "MaListe.xlsx");
            var candidats = await _candidatService.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, User, 500);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Candidats");

            // Clear existing data
            //worksheet.Cells.Clear();

            //Add the headers
            worksheet.Cells[1, 1].Value = "Nom et prénom";
            worksheet.Cells[1, 2].Value = "Date d'adèration";
            worksheet.Cells[1, 3].Value = "Numéro de téléphone";
            worksheet.Cells[1, 4].Value = "Sexe";
            worksheet.Cells[1, 5].Value = "Adresse";
            worksheet.Cells[1, 6].Value = "Email";
            worksheet.Cells[1, 7].Value = "Découverte de la plateforme";
            worksheet.Cells[1, 8].Value = "Situation familiale";
            worksheet.Cells[1, 9].Value = "CIN";
            worksheet.Cells[1, 10].Value = "Date de naissance";
            worksheet.Cells[1, 11].Value = "Status ";
            worksheet.Cells[1, 12].Value = "Orientation ";
            worksheet.Cells[1, 13].Value = "Commentaire ";

            //Add some items...
            for (int i = 0; i < candidats.Count; i++)
            {
                worksheet.Cells["A" + (i + 2)].Value = candidats[i].Nom + ", " + candidats[i].Prenom;
                worksheet.Cells["B" + (i + 2)].Value = candidats[i].DateAderation.HasValue == true ? candidats[i].DateAderation.Value.ToShortDateString() : "";
                worksheet.Cells["C" + (i + 2)].Value = candidats[i].Telephone;
                worksheet.Cells["D" + (i + 2)].Value = candidats[i].Sexe;
                worksheet.Cells["E" + (i + 2)].Value = candidats[i].Adresse;
                worksheet.Cells["F" + (i + 2)].Value = candidats[i].Email;
                worksheet.Cells["G" + (i + 2)].Value = candidats[i].DecouvertePlateForme;
                worksheet.Cells["H" + (i + 2)].Value = candidats[i].SituationFamilial;
                worksheet.Cells["I" + (i + 2)].Value = candidats[i].CIN;
                worksheet.Cells["J" + (i + 2)].Value = candidats[i].DateNaissance;
                worksheet.Cells["K" + (i + 2)].Value = candidats[i].Statut;
                worksheet.Cells["L" + (i + 2)].Value = candidats[i].Orientation;
                worksheet.Cells["M" + (i + 2)].Value = candidats[i].Commentaire;
            }

            //Ok now format the values;
            using (var range = worksheet.Cells[1, 1, 1, (candidats.Count + 1)])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //range.Worksheet.Rows.Height = 30;
            }

            worksheet.Cells["A2:M" + candidats.Count + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:M" + candidats.Count + 1].Style.Font.Bold = true;

            //Create an autofilter for the range
            worksheet.Cells["A1:M" + candidats.Count + 1].AutoFilter = true;

            worksheet.Cells["A2:M" + candidats.Count + 1].Style.Numberformat.Format = "@";   //Format as text

            //There is actually no need to calculate, Excel will do it for you, but in some cases it might be useful. 
            //For example if you link to this workbook from another workbook or you will open the workbook in a program that hasn't a calculation engine or 
            //you want to use the result of a formula in your program.
            worksheet.Calculate();

            worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

            // lets set the header text 
            worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Candidats";
            // add the page number to the footer plus the total number of pages
            worksheet.HeaderFooter.OddFooter.RightAlignedText =
                string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
            // add the sheet name to the footer
            worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
            // add the file path to the footer
            worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

            worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
            worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

            // set some document properties
            package.Workbook.Properties.Title = "Listes des candidats";
            package.Workbook.Properties.Author = "Moussaoui badr eddine";
            package.Workbook.Properties.Comments = "Candidats";

            // set some extended property values
            package.Workbook.Properties.Company = "B.Moussaoui";

            // set some custom property values
            package.Workbook.Properties.SetCustomPropertyValue("Checked by", "B.M");
            package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "B.M");

            //await package.SaveAsync();

            //var xlFile = new FileInfo(path);
            // save our new workbook in the output directory and we are done!
            //package.SaveAs(xlFile);


            //stream.Position = 0;
            string excelName = $"MaListe.xlsx";
            return File(package.GetAsByteArray(), "application/octet-stream", excelName);

        }

        [HttpGet]
        public IActionResult ImportExcelFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelFile(IFormFile file)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream, CancellationToken.None);
                    using var package = new ExcelPackage(stream);

                    //Lire la deuxième feuille
                    foreach (var worksheet in package.Workbook.Worksheets.Skip(1))
                    {
                        if (worksheet.Dimension == null)
                        {
                            Debug.WriteLine("Table is empty");
                            TempData["Message"] = "ER2";
                        }
                        //Lecture du nom de la feuille
                        var name = worksheet.Name.Trim();
                        //Compter les lignes de la feuille
                        var rowCount = worksheet.Dimension!.Rows;
                        for (var row = 3; row <= rowCount; row++)
                        {
                            if (worksheet.Cells[row, 1].Value == null) break;
                            //3 ème ligne et 1 ère colonne
                            var c1 = worksheet.Cells[row, 1].Value.ToString()!.Trim();
                            var c2 = worksheet.Cells[row, 2].Value.ToString()!.Trim();
                        }
                    }
                    TempData["Excel"] = "Fichier importé avec succès";
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Excel"] = ex.Message;
                return View();
            }
        }


        //public IActionResult DownloadFile(string filename)
        //{
        //    var memory = DownloadSinghFile(filename, "Canevas");
        //    return File(memory.ToArray(), "application/vnd.ms-excel", filename);
        //}

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

        public async Task<IActionResult> Index(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            try
            {
                // Vérification si tous les paramètres sont nuls ou vides
                if (string.IsNullOrEmpty(SearchNom) &&
                    string.IsNullOrEmpty(SearchPrenom) &&
                    string.IsNullOrEmpty(SearchDate) &&
                    string.IsNullOrEmpty(SearchDateO) &&
                    !pageNumber.HasValue &&
                    string.IsNullOrEmpty(SearchCIN) &&
                    string.IsNullOrEmpty(SearchStatut) &&
                    string.IsNullOrEmpty(OrientationFilter))
                {
                    // Si tous les paramètres sont nuls ou vides, retournez simplement la vue sans appeler GetCandidats.
                    return View();
                }

                ViewData["CINSortParm"] = SearchCIN;
                ViewData["OrientationSortParm"] = OrientationFilter;
                ViewData["StatutSortParm"] = SearchStatut;
                ViewData["CurrentFilterN"] = SearchNom;
                ViewData["CurrentFilterP"] = SearchPrenom;
                ViewData["CurrentFilterD"] = SearchDate;
                ViewData["CurrentFilterDO"] = SearchDateO;

                var result = await _candidatService.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, User, 15);
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["ExceptionMessage"] = "ex => " + ex.Message;
                return RedirectToAction("Exception");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateClient model)
        {
            try
            {
                var response = await _candidatService.AddCandidat(model.Candidat);
                if (model.DocumentModel != null)
                {
                    var responseDocument = await _candidatService.AddDocument(model.DocumentModel, response.ID);
                }
                if (model.DiplomeModel != null)
                {
                    var responseDiplome = await _candidatService.AddDiplome(model.DiplomeModel, response.ID);
                }

                return RedirectToAction("Index", "Candidats");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    TempData["ExceptionMessage"] = "ex => " + ex.Message;
                    TempData["InnerException"] = "ex => " + ex.InnerException.Message;
                    return RedirectToAction("Exception");
                }
                else
                {
                    TempData["ExceptionMessage"] = "ex => " + ex.Message;
                    return RedirectToAction("Exception");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ID)
        {
            var candidat = await _candidatService.GetCandidat(ID);
            return View(candidat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client model)
        {
            var response = await _candidatService.EditCandidat(new Client
            {
                ClientID = model.ClientID,
                Nom = model.Nom,
                Prenom = model.Prenom,
                Adresse = model.Adresse,
                DateNaissance = model.DateNaissance,
                DecouvertePlateForme = model.DecouvertePlateForme,
                Sexe = model.Sexe,
                Email = model.Email,
                SituationFamilial = model.SituationFamilial,
                Statut = model.Statut,
                Telephone = model.Telephone,
                CIN = model.CIN,
                DateAderation = model.DateAderation,
                Orientation = model.Orientation,
                Created = DateTime.Now,
                File = model.File,
                ImageUrl = model.ImageUrl,
                Commentaire = model.Commentaire
            });
            var responseDocument = await _candidatService.EditDocument(model.Documents);
            var responseDiplome = await _candidatService.EditDiplome(model.Diplomes);

            if (response.Success && responseDocument.Success && responseDiplome.Success) return RedirectToAction("Index", "Candidats");

            else
            {
                ViewData["MessageDiplome"] = responseDiplome.Message;
                Console.WriteLine(responseDiplome.Message2);
                ViewData["MessageDocument"] = responseDocument.Message;
                Console.WriteLine(responseDocument.Message2);
                ViewData["Message"] = response.Message;
                Console.WriteLine(response.Message2);
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


        [Authorize(Roles = "Admin, Gestionnaire")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            var result = await _candidatService.Delete(id);
            if (result.Success) return RedirectToAction("Index", new
            {
                SearchNom = SearchNom,
                SearchPrenom = SearchPrenom,
                SearchDate = SearchDate,
                SearchCIN = SearchCIN,
                SearchDateO = SearchDateO,
                SearchStatut = SearchStatut,
                OrientationFilter = OrientationFilter,
                pageNumber = pageNumber
            });
            else return RedirectToAction("Index", new { Error = "ErreurSuppression" });
        }

        [HttpGet]
        public async Task<IActionResult> Orientation(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter)
        {
            // Vérification si tous les paramètres sont nuls ou vides
            if (string.IsNullOrEmpty(SearchNom) &&
                string.IsNullOrEmpty(SearchPrenom) &&
                string.IsNullOrEmpty(SearchDate) &&
                string.IsNullOrEmpty(SearchDateO) &&
                !pageNumber.HasValue &&
                string.IsNullOrEmpty(SearchCIN) &&
                string.IsNullOrEmpty(SearchStatut) &&
                string.IsNullOrEmpty(OrientationFilter))
            {
                // Si tous les paramètres sont nuls ou vides, retournez simplement la vue sans appeler GetCandidatsO.
                return View();
            }

            return View(await _candidatService.GetCandidatsO(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, User, 10));
        }


        [HttpGet]
        public async Task<string> AffecterOrientation(int ClientID, int valide)
        {
            var client = await _candidatService.GetCandidat(ClientID);
            if (valide == 1)
            {
                client.Orientation = Domain.Entities.Orientation.Orienté;
            }
            else if (valide == 2)
            {
                client.Orientation = Domain.Entities.Orientation.NonOrienté;
            }
            else if (valide == 4)
            {
                client.Orientation = Domain.Entities.Orientation.Financé;
            }
            else
            {
                client.Orientation = Domain.Entities.Orientation.Refusé;
            }

            await _candidatService.EditCandidatNotMapProfile(client);

            return "Ok";
        }


    }
}
