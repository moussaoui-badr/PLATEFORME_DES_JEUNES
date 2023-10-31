using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using OpenHtmlToPdf;
using Repository.IRepositories;
using Service.IService;
using Service.StaticHelperService;
using System.Globalization;

namespace Service.Service
{
#nullable disable
    public class CandidatINDHService : ICandidatINDHService
    {
        private readonly ICandidatINDHRepository _candidatRepository;
        private readonly IWebHostEnvironment _hosting;
        public CandidatINDHService(ICandidatINDHRepository candidatRepository, IWebHostEnvironment hosting)
        {
            _candidatRepository = candidatRepository;
            _hosting = hosting;
        }
        public async Task<PaginatedList<ClientFinance>> GetCandidats(string SearchPG, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, int PageSize)
        {
            return await _candidatRepository.GetCandidats(SearchPG, SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, PageSize);
        }

        public async Task<Response> AddCandidat(CandidatINDHViewModel client)
        {
            if (client.ImageUrl != null)
            {
                client.ImageUrlString = RemoveCaractereSpeciaux.removeCaractereSpeciaux(client.ImageUrl.FileName);
                await CopyFile.CopyFileLocation(client.ImageUrlString, "Uploads", client.ImageUrl, _hosting);
            }
            else client.ImageUrlString = client.Sexe == SexeC.Homme ? "avatar.png" : "avatar-woman.png";

            return await _candidatRepository.AddCandidat(new ClientFinance
            {
                Nom = client.Nom,
                Prenom = client.Prenom,
                Adresse = client.Adresse,
                DateNaissance = client.DateNaissance,
                Sexe = SexeC.Homme,
                Email = client.Email,
                Telephone = client.Telephone,
                CIN = client.CIN,
                DateAderation = client.DateAderation,
                Created = DateTime.Now,
                ImageUrl = client.ImageUrlString,
                MontantApportPersonnel = client.MontantApportPersonnel,
                MontantProjet = client.MontantProjet,
                MontantINDH = client.MontantINDH,
                Commentaire = client.Commentaire,
                PlateformeGestionnaire = client.PlateformeGestionnaire,
                ApportPersonnelConfirme = client.ApportPersonnelConfirme
            });
        }
        public async Task<Response> AddIndh(List<Domain.Entities.INDH> INDH, int ID, PlateformeGestionnaire pg)
        {
            return await _candidatRepository.AddIndh(INDH, ID, pg);
        }

        public async Task<Response> AddDocument(List<DocumentINDH> documents, int ID)
        {
            try
            {
                var documentsList = new List<DocumentINDH>();
                if (documents != null)
                {
                    foreach (var item in documents)
                    {
                        if (item.File != null)
                        {
                            item.NomFichier = RemoveCaractereSpeciaux.removeCaractereSpeciaux(item.File.FileName);
                            await CopyFile.CopyFileLocation(item.NomFichier, "Documents", item.File, _hosting);
                        }
                        documentsList.Add(new DocumentINDH
                        {
                            Designation = item.Designation,
                            NomFichier = item.NomFichier,
                            ClientID = ID,

                        });
                    }
                    return await _candidatRepository.AddDocument(documentsList);
                }
                else return new Response
                {
                    Success = false,
                    Message = "Aucun document trouvé."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Success = false,
                    Message = "Une erreur est survenu.",
                    Message2 = ex.Message.ToString()
                };
            }

        }


        public async Task<Response> EditCandidat(ClientFinance client)
        {
            if (client.File != null)
            {
                client.ImageUrl = RemoveCaractereSpeciaux.removeCaractereSpeciaux(client.File.FileName);
                await CopyFile.CopyFileLocation(client.ImageUrl, "Uploads", client.File, _hosting);
                client.File = null;
            }

            return await _candidatRepository.EditCandidat(client);
        }

        public async Task<Response> EditCandidatNotMapProfile(ClientFinance client)
        {
            return await _candidatRepository.EditCandidat(client);
        }

        public async Task<Response> EditDocument(ICollection<DocumentINDH> documents)
        {
            try
            {
                var documentsList = new List<DocumentINDH>();
                if (documents != null)
                {
                    foreach (var item in documents)
                    {
                        if (item.File != null)
                        {
                            item.NomFichier = RemoveCaractereSpeciaux.removeCaractereSpeciaux(item.File.FileName);
                            await CopyFile.CopyFileLocation(item.NomFichier, "Documents", item.File, _hosting);
                        }
                        if (item.DocumentINDHID != 0)
                        {
                            documentsList.Add(new DocumentINDH
                            {
                                Designation = item.Designation,
                                NomFichier = item.NomFichier,
                                ClientID = item.ClientID,
                                DocumentINDHID = item.DocumentINDHID
                            });
                        }
                        else
                        {
                            documentsList.Add(new DocumentINDH
                            {
                                Designation = item.Designation,
                                NomFichier = item.NomFichier,
                                ClientID = item.ClientID,
                            });
                        }

                    }
                    return await _candidatRepository.EditDocument(documentsList);
                }
                else return new Response
                {
                    Success = true,
                    Message = "Aucun document trouvé."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Success = false,
                    Message = "Une erreur est survenu.",
                    Message2 = ex.Message.ToString()
                };
            }

        }

        public async Task<ClientFinance> GetCandidat(int ID)
        {
            return await _candidatRepository.GetCandidat(ID);
        }

        public async Task<CreateClientINDH> GetCandidatEdit(int ID)
        {
            var candidat = await _candidatRepository.GetCandidat(ID);

            var createClientINDH = new CreateClientINDH
            {
                INDHs = candidat.INDHS.ToList(),
                Candidat = new CandidatINDHViewModel
                {
                    ClientID = candidat.ClientFinanceID,
                    Nom = candidat.Nom,
                    Prenom = candidat.Prenom,
                    Adresse = candidat.Adresse,
                    DateNaissance = candidat.DateNaissance,
                    Sexe = candidat.Sexe,
                    Email = candidat.Email,
                    Telephone = candidat.Telephone,
                    CIN = candidat.CIN,
                    DateAderation = candidat.DateAderation,
                    Created = candidat.Created,
                    ImageUrlString = candidat.ImageUrl,
                    MontantApportPersonnel = candidat.MontantApportPersonnel,
                    MontantProjet = candidat.MontantProjet,
                    MontantINDH = candidat.MontantINDH,
                    Commentaire = candidat.Commentaire,
                    PlateformeGestionnaire = candidat.PlateformeGestionnaire,
                    ApportPersonnelConfirme = candidat.ApportPersonnelConfirme
                },
                DocumentModel = candidat.Documents.ToList()
            };
            return createClientINDH;

        }

        public async Task<Byte[]> PrintPdf(int id)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            var client = await _candidatRepository.GetCandidat(id);
            string Diplomes = null;
            string diplome = null;
            string espaceDiplome = null;

            if (client.INDHS.Count > 0)
            {
                espaceDiplome = "";
                diplome = "INDH :";
                foreach (var it in client.INDHS)
                {
                    Diplomes += "<tr><td data-column=\"Materiel\">" + it.Materiel + "</td><td data-column=\"Montant\">" + it.MontantAcquisition.ToString("yyyy-MM-dd") + "</td><td data-column=\"Etat\">" + it.Etat.ToString() + "</td></tr></br>";
                }
            }
            else
            {
                espaceDiplome = "";
                diplome = "";
                Diplomes = "";
            }


            string uploads = Path.Combine(_hosting.WebRootPath, "Templates");
            string fullPath = Path.Combine(uploads, "EmailTemplate2.html");
            var str = new StreamReader(fullPath);

            var TemplateHtml = str.ReadToEnd();
            str.Close();

            string uploadsImage = Path.Combine(_hosting.WebRootPath, "Uploads");
            string fullPathImage = Path.Combine(uploadsImage, client.ImageUrl);
            TemplateHtml = TemplateHtml
                .Replace("[Nom]", client.Nom)
                .Replace("[Prenom]", client.Prenom)
                .Replace("[Adresse]", client.Adresse)
                .Replace("[DateNaissance]", client.DateNaissance.ToString("yyyy-MM-dd"))
                .Replace("[Cin]", client.CIN)
                .Replace("[ImageUrl]", fullPathImage)
                .Replace("[DateAderation]", client.DateAderation?.ToString("yyyy-MM-dd"))
                .Replace("[Email]", client.Email)
                .Replace("[Sexe]", client.Sexe.ToString())
            .Replace("[MontantApportPersonnel]", client.MontantApportPersonnel.ToString("#,0.00", nfi))
            .Replace("[MontantINDH]", client.MontantINDH.ToString("#,0.00", nfi))
            .Replace("[MontantProjet]", client.MontantProjet.ToString("#,0.00", nfi))
            .Replace("[Diplomes]", Diplomes)
            .Replace("[diplome]", diplome)
            .Replace("[EspaceDiplome]", espaceDiplome)
            .Replace("[PlateformeGestionnaire]", client.PlateformeGestionnaire.ToString());


            var pdf = Pdf.From(TemplateHtml)
                    .OfSize(PaperSize.A4)
                    .WithMargins(0.Centimeters())
                    .Landscape()
                    .Content();

            // Retourner le PDF généré en tant que fichier à télécharger
            return pdf;
        }

        public async Task<Response> Delete(int id)
        {
            return await _candidatRepository.Delete(id);
        }

        public async Task<Response> EditIndh(List<Domain.Entities.INDH> INDH, PlateformeGestionnaire pg)
        {
            return await _candidatRepository.EditIndh(INDH, pg);
        }

        public async Task DeleteIndh(int ID)
        {
            await _candidatRepository.DeleteIndh(ID);
        }

        public async Task<IEnumerable<INDH_FiltreModel>> FiltreMaterielIndh(int? EtatMateriel, int? ApportPersonnel)
        {
            return await _candidatRepository.FiltreMaterielIndh(EtatMateriel, ApportPersonnel);
        }
    }
}
