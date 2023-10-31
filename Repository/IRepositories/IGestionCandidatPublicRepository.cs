using Domain.Entities;
using Domain.Models;
using System.Security.Claims;

namespace Repository.IRepositories
{
    public interface IGestionCandidatPublicRepository
    {
        Task<Client> GetCandidat(int Id);
        Task Delete(int Id);
        Task Valider(int Id);
        Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims);
    }
}
