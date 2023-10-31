using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Repository.IRepositories;
using Service.IService;
using Service.StaticHelperService;

namespace Service.Service
{
#nullable disable
    public class CandidatPublicService : ICandidatPublicService
    {
        private readonly ICandidatPublicRepository _candidatPublicRepository;
        private readonly IWebHostEnvironment _hosting;
        public CandidatPublicService(ICandidatPublicRepository candidatPublicRepository, IWebHostEnvironment hosting)
        {
            _candidatPublicRepository = candidatPublicRepository;
            _hosting = hosting;
        }

        public async Task<Response> AddCandidat(CandidatViewModel client)
        {
            if (client.ImageUrl != null)
            {
                client.ImageUrlString = RemoveCaractereSpeciaux.removeCaractereSpeciaux(client.ImageUrl.FileName);
                await CopyFile.CopyFileLocation(client.ImageUrlString, "Uploads", client.ImageUrl, _hosting);
            }
            else client.ImageUrlString = client.Sexe == Sexe.Homme ? "avatar.png" : "avatar-woman.png";

            return await _candidatPublicRepository.AddCandidat(new Client
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
                ImageUrl = client.ImageUrlString,
                Public = true,
                Oriente = false
            }) ;
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
                    return await _candidatPublicRepository.AddDocument(documentsList, ID);
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

        public async Task<Response> AddDiplome(List<DiplomeModel> DiplomeModel, int ID)
        {
            try
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
                    return await _candidatPublicRepository.AddDiplome(diplomesList, ID);
                }
                return new Response
                {
                    Success = false,
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

    }
}
