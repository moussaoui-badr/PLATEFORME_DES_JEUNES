using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Repository.IRepositories
{
    public interface ICandidatINDHRepository
    {
        Task<IEnumerable<INDH_FiltreModel>> FiltreMaterielIndh(int? EtatMateriel, int? ApportPersonnel);
        Task DeleteIndh(int ID);
        Task<Response> EditIndh(List<INDH> INDH, PlateformeGestionnaire pg);
        Task<Response> Delete(int id);
        Task<ClientFinance> GetCandidat(int ID);
        Task<Response> EditDocument(List<DocumentINDH> documents);
        Task<Response> EditCandidat(ClientFinance client);
        Task<Response> AddDocument(List<DocumentINDH> documents);
        Task<Response> AddCandidat(ClientFinance client);
        Task<PaginatedList<ClientFinance>> GetCandidats(string SearchPG, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, int PageSize);
        Task<Response> AddIndh(List<INDH> INDH, int ID, PlateformeGestionnaire pg);
    }
}
