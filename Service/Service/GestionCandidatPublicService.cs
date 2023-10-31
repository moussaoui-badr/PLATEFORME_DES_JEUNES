using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OpenHtmlToPdf;
using Repository.IRepositories;
using Service.IService;
using System.Security.Claims;


namespace Service.Service
{
#nullable disable
    public class GestionCandidatPublicService : IGestionCandidatPublicService
    {
        private readonly IGestionCandidatPublicRepository _gestionCandidatPublicRepository;
        private readonly IWebHostEnvironment _hosting;

        public GestionCandidatPublicService(IGestionCandidatPublicRepository gestionCandidatPublicRepository, IWebHostEnvironment hosting)
        {
            _gestionCandidatPublicRepository = gestionCandidatPublicRepository;
            _hosting = hosting;
        }

        public async Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims)
        {
            return await _gestionCandidatPublicRepository.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, claims);
        }

        public async Task Valider(int Id)
        {
            await _gestionCandidatPublicRepository.Valider(Id);
        }
        public async Task Delete(int Id)
        {
            await _gestionCandidatPublicRepository.Delete(Id);
        }
        public async Task<FileStreamResult> PrintPdf(int id)
        {
            var client = await _gestionCandidatPublicRepository.GetCandidat(id);

            string uploads = Path.Combine(_hosting.WebRootPath, "Templates");
            string fullPath = Path.Combine(uploads, "EmailTemplate.html");
            var str = new StreamReader(fullPath);

            var TemplateHtml = str.ReadToEnd();
            str.Close();

            string Diplomes = null;
            string Formations = null;
            string diplome = null;
            string formation = null;
            string espace = null;


            if (client.Diplomes != null)
            {
                espace = "</br></br></br></br></br></br></br></br></br>";
                diplome = "Diplomes :";
                foreach (var it in client.Diplomes)
                {
                    Diplomes += "<tr><td data-column=\"Nom du diplome\">" + it.DiplomeName + "</td><td data-column=\"Branche\">" + it.Branche + "</td><td data-column=\"Nom de l'institut\">" + it.InstitutName + "</td><td data-column=\"Date d'obtention du diplome\">" + it.DateObtentionDiplome.ToString("MM/dd/yyyy") + "</td></tr></br>";
                }
            }
            else
            {
                espace = "";
                diplome = "";
                Diplomes = "";
            }

            string uploadsImage = Path.Combine(_hosting.WebRootPath, "Uploads");
            string fullPathImage = Path.Combine(uploadsImage, client.ImageUrl);
            TemplateHtml = TemplateHtml
                .Replace("[Nom]", client.Nom)
                .Replace("[Prenom]", client.Prenom)
                .Replace("[Adresse]", client.Adresse)
                .Replace("[DateNaissance]", client.DateNaissance.GetValueOrDefault().ToString("yyyy-MM-dd"))
                .Replace("[Cin]", client.CIN)
                .Replace("[ImageUrl]", fullPathImage)
                .Replace("[DecouvertePlateforme]", client.DecouvertePlateForme)
                .Replace("[DateAderation]", client.DateAderation?.ToString("yyyy-MM-dd"))
                .Replace("[Email]", client.Email)
                .Replace("[Sexe]", client.Sexe.ToString())
                .Replace("[Oriente]", client.Oriente.GetValueOrDefault() ? "Oui" : "Non")
                .Replace("[Diplome]", diplome)
                .Replace("[Formation]", formation)
                .Replace("[Diplomes]", Diplomes)
                .Replace("[Formations]", Formations)
                .Replace("[Espace]", espace);

            var pdf = Pdf.From(TemplateHtml)
                .OfSize(PaperSize.A4)
                .WithMargins(0.Centimeters())
                .Landscape()
                .Content();


            string uploadsImagePdf = Path.Combine(_hosting.WebRootPath, "Fiches");
            string fullPathImagePdf = Path.Combine(uploadsImagePdf, "fiche.pdf");
            System.IO.File.WriteAllBytes(fullPathImagePdf, pdf);

            var stream = new FileStream(fullPathImagePdf, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<Client> GetCandidat(int Id)
        {
            return await _gestionCandidatPublicRepository.GetCandidat(Id);
        }
    }
}
