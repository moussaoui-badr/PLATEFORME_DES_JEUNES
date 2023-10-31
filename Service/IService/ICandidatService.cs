using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using System.Security.Claims;

namespace Service.IService
{
    public interface ICandidatService
    {
        Task<Response> Delete(int id);
        Task<byte[]> PrintPdf(int id);
        Task<Client> GetCandidat(int ID);
        Task<Response> EditDiplome(ICollection<Diplome> DiplomeModel);
        Task<Response> EditDocument(ICollection<Document> documents);
        Task<Response> EditCandidat(Client client);
        Task<Response> EditCandidatNotMapProfile(Client client);
        Task<Response> AddDiplome(List<DiplomeModel> DiplomeModel, int ID);
        Task<Response> AddDocument(List<DocumentModel> documents, int ID);
        Task<Response> AddCandidat(CandidatViewModel client);
        Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize);
        Task<PaginatedList<Client>> GetCandidatsO(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize);
    }
}
