using Domain.Entities;
using Domain.Models;
using Repository.IRepositories;
using Service.IService;
using System.Security.Claims;

namespace Service.Service
{
    public class FormationService : IFormationService
    {
        private readonly IFormationRepository _formationRepository;

        public FormationService(IFormationRepository formationRepository)
        {
            _formationRepository = formationRepository;
        }

        public async Task AddFormation(Formation formation)
        {
            await _formationRepository.AddFormation(formation);
        }

        public async Task Delete(int id)
        {
            await _formationRepository.Delete(id);
        }

        public async Task Edit(Formation formation)
        {
            await _formationRepository.Edit(formation);
        }

        public async Task EditAffectation(int formationId, int clientId, bool valide)
        {
            await _formationRepository.EditAffectation(formationId, clientId, valide);
        }

        public async Task<PaginatedList<Client>> getCandidats(int FormationID, string Theme, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims)
        {
            return await _formationRepository.getCandidats(FormationID, Theme, SearchNom, SearchPrenom, SearchDate, SearchDateO, pageNumber, SearchCIN, SearchStatut, OrientationFilter, claims);
        }

        public async Task<Formation> getFormation(int Id)
        {
            return await _formationRepository.getFormation(Id);
        }

        public async Task<PaginatedList<Formation>> getFormations(int? pageNumber, ClaimsPrincipal claims, string Theme, string Animateur, int Duree, string SearchDate, string SearchDateO)
        {
            return await _formationRepository.getFormations(pageNumber, claims, Theme, Animateur, Duree, SearchDate, SearchDateO);
        }
    }
}
