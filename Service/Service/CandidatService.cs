using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using OpenHtmlToPdf;
using Repository.IRepositories;
using Service.IService;
using Service.StaticHelperService;
using System.Security.Claims;

namespace Service.Service
{
#nullable disable
    public class CandidatService : ICandidatService
    {
        private readonly ICandidatRepository _candidatRepository;
        private readonly IWebHostEnvironment _hosting;
        public CandidatService(ICandidatRepository candidatRepository, IWebHostEnvironment hosting)
        {
            _candidatRepository = candidatRepository;
            _hosting = hosting;
        }
        public async Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize)
        {
            return await _candidatRepository.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, claims, PageSize);
        }

        public async Task<Response> AddCandidat(CandidatViewModel client)
        {
            if (client.ImageUrl != null)
            {
                client.ImageUrlString = RemoveCaractereSpeciaux.removeCaractereSpeciaux(client.ImageUrl.FileName);
                await CopyFile.CopyFileLocation(client.ImageUrlString, "Uploads", client.ImageUrl, _hosting);
            }
            else client.ImageUrlString = client.Sexe == Sexe.Homme ? "avatar.png" : "avatar-woman.png";

            return await _candidatRepository.AddCandidat(new Client
            {
                Nom = client.Nom,
                Prenom = client.Prenom,
                Adresse = client.Adresse,
                DateNaissance = client.DateNaissance,
                DecouvertePlateForme = client.DecouvertePlateForme,
                Sexe = client.Sexe,
                Email = client.Email,
                SituationFamilial = client.SituationFamilial,
                Statut = client.Statut,
                Telephone = client.Telephone,
                CIN = client.CIN,
                DateAderation = client.DateAderation,
                Orientation = Orientation.NonOrienté,
                Created = DateTime.Now,
                ImageUrl = client.ImageUrlString
            });
        }

        public async Task<Response> AddDocument(List<DocumentModel> documents, int ID)
        {
            try
            {
                var documentsList = new List<Document>();
                if (documents != null)
                {
                    foreach (var item in documents)
                    {
                        if (item.File != null)
                        {
                            item.NomFichier = RemoveCaractereSpeciaux.removeCaractereSpeciaux(item.File.FileName);
                            await CopyFile.CopyFileLocation(item.NomFichier, "Documents", item.File, _hosting);
                        }
                        documentsList.Add(new Document
                        {
                            Designation = item.Designation,
                            NomFichier = item.NomFichier,
                            ClientID = ID,
                        });
                    }
                    return await _candidatRepository.AddDocument(documentsList, ID);
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

        public async Task<Response> AddDiplome(List<DiplomeModel> DiplomeModel, int ID)
        {

                var diplomesList = new List<Diplome>();
                if (DiplomeModel != null)
                {
                    foreach (var item in DiplomeModel)
                    {
                        diplomesList.Add(new Diplome
                        {
                            DateObtentionDiplome = item.DateObtentionDiplome,
                            DiplomeName = item.DiplomeName,
                            InstitutName = item.InstitutName,
                            Branche = item.Branche,
                            ClientID = ID,
                        });
                    }
                    return await _candidatRepository.AddDiplome(diplomesList, ID);
                }
                return new Response
                {
                    Success = false,
                    Message = "Aucun diplome trouvé."
                };
            
        }

        public async Task<Response> EditCandidat(Client client)
        {
            if (client.File != null)
            {
                client.ImageUrl = RemoveCaractereSpeciaux.removeCaractereSpeciaux(client.File.FileName);
                await CopyFile.CopyFileLocation(client.ImageUrl, "Uploads", client.File, _hosting);
                client.File = null;
            }

            return await _candidatRepository.EditCandidat(client);
        }

        public async Task<Response> EditCandidatNotMapProfile(Client client)
        {
            return await _candidatRepository.EditCandidat(client);
        }

        public async Task<Response> EditDocument(ICollection<Document> documents)
        {
            try
            {
                var documentsList = new List<Document>();
                if (documents != null)
                {
                    foreach (var item in documents)
                    {
                        if (item.File != null)
                        {
                            item.NomFichier = RemoveCaractereSpeciaux.removeCaractereSpeciaux(item.File.FileName);
                            await CopyFile.CopyFileLocation(item.NomFichier, "Documents", item.File, _hosting);
                        }
                        if (item.DocumentID != 0)
                        {
                            documentsList.Add(new Document
                            {
                                Designation = item.Designation,
                                NomFichier = item.NomFichier,
                                ClientID = item.ClientID,
                                DocumentID = item.DocumentID
                            });
                        }
                        else
                        {
                            documentsList.Add(new Document
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

        public async Task<Response> EditDiplome(ICollection<Diplome> DiplomeModel)
        {
            try
            {
                var diplomesList = new List<Diplome>();
                if (DiplomeModel != null)
                {
                    foreach (var item in DiplomeModel)
                    {
                        if (item.DiplomeID != 0)
                        {
                            diplomesList.Add(new Diplome
                            {
                                DateObtentionDiplome = item.DateObtentionDiplome,
                                DiplomeName = item.DiplomeName,
                                InstitutName = item.InstitutName,
                                Branche = item.Branche,
                                ClientID = item.ClientID,
                                DiplomeID = item.DiplomeID,
                            });
                        }
                        else
                        {
                            diplomesList.Add(new Diplome
                            {
                                DateObtentionDiplome = item.DateObtentionDiplome,
                                DiplomeName = item.DiplomeName,
                                InstitutName = item.InstitutName,
                                Branche = item.Branche,
                                ClientID = item.ClientID,
                            });
                        }
                    }
                    return await _candidatRepository.EditDiplome(diplomesList);
                }
                return new Response
                {
                    Success = true,
                    Message = "Aucun diplome trouvé."
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

        public async Task<Client> GetCandidat(int ID)
        {
            return await _candidatRepository.GetCandidat(ID);
        }

        public async Task<Byte[]> PrintPdf(int id)
        {
            var client = await _candidatRepository.GetCandidat(id);

            string uploads = Path.Combine(_hosting.WebRootPath, "Templates");
            string fullPath = Path.Combine(uploads, "EmailTemplate2.html");
            var str = new StreamReader(fullPath);

            var TemplateHtml = str.ReadToEnd();
            str.Close();

            string Diplomes = null;
            string Formations = null;
            string diplome = null;
            string formation = null;
            string espace = null;
            string espaceDiplome = null;
            string espaceFormation = null;


            if (client.Diplomes.Count > 0)
            {
                diplome = "Diplomes :";
                foreach (var it in client.Diplomes)
                {
                    Diplomes += "<tr><td data-column=\"Nom du diplome\">" + it.DiplomeName + "</td><td data-column=\"Branche\">" + it.Branche + "</td><td data-column=\"Nom de l'institut\">" + it.InstitutName + "</td><td data-column=\"Date d'obtention du diplome\">" + it.DateObtentionDiplome.ToString("MM/dd/yyyy") + "</td></tr></br>";
                }
            }
            else
            {
                espaceDiplome = "";
                diplome = "";
                Diplomes = "";
            }
            if (client.InscriptionFormation.Count > 0)
            {
                if (client.Diplomes.Count > 0)
                {
                    espaceFormation = "";
                }
                else
                {
                    espaceFormation = "</br></br></br></br>";
                }

                formation = "Formations :";

                foreach (var it in client.InscriptionFormation)
                {
                    Formations += "<tr><td data-column=\"Theme\">" + it.Formation.Theme + "</td><td data-column=\"Date de formation\">" + it.Formation.DateFormation.ToString("MM/dd/yyyy") + "</td><td data-column=\"Duree de formation\">" + it.Formation.DureeFormation + "</td></tr></br>";
                }
            }
            else
            {
                espaceFormation = "";
                formation = "";
                Formations = "";
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
                .Replace("[Espace]", espace)
                .Replace("[EspaceDiplome]", espaceDiplome)
                .Replace("[EspaceFormation]", espaceFormation);

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

        public async Task<PaginatedList<Client>> GetCandidatsO(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize)
        {
            return await _candidatRepository.GetCandidats(SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, claims, PageSize);
        }
    }
}
