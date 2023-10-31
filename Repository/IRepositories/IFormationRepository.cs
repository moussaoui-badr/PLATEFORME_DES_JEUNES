using Domain.Entities;
using Domain.Models;
using System.Security.Claims;

namespace Repository.IRepositories
{
    public interface IFormationRepository
    {
        Task<PaginatedList<Formation>> getFormations(int? pageNumber, ClaimsPrincipal claims, string Theme, string Animateur, int Duree, string SearchDate, string SearchDateO);
        Task<Formation> getFormation(int Id);
        Task AddFormation(Formation formation);
        Task Edit(Formation formation);
        Task Delete(int id);
        Task<PaginatedList<Client>> getCandidats(int FormationID, string Theme, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims);
        Task EditAffectation(int formationId, int clientId, bool valide);
    }
}
