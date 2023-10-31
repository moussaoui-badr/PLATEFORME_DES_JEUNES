using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using System.Security.Claims;

namespace Repository.IRepositories
{
    public interface ICandidatRepository
    {
        Task<Response> Delete(int id);
        Task<Client> GetCandidat(int ID);
        Task<Response> EditDiplome(List<Diplome> diplome);
        Task<Response> EditDocument(List<Document> documents);
        Task<Response> EditCandidat(Client client);
        Task<Response> AddDiplome(List<Diplome> Diplome, int ID);
        Task<Response> AddDocument(List<Document> documents, int ID);
        Task<Response> AddCandidat(Client client);
        Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int? PageSize);
        Task<PaginatedList<Client>> GetCandidatsO(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize);
    }
}
