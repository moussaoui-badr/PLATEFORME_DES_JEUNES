using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Service.IService
{
    public interface ICandidatINDHService
    {
        Task<IEnumerable<INDH_FiltreModel>> FiltreMaterielIndh(int? EtatMateriel, int? ApportPersonnel);
        Task DeleteIndh(int ID);
        Task<Response> EditIndh(List<INDH> INDH, PlateformeGestionnaire pg);
        Task<CreateClientINDH> GetCandidatEdit(int ID);
        Task<Response> Delete(int id);
        Task<byte[]> PrintPdf(int id);
        Task<ClientFinance> GetCandidat(int ID);
        Task<Response> EditDocument(ICollection<DocumentINDH> documents);
        Task<Response> EditCandidat(ClientFinance client);
        Task<Response> EditCandidatNotMapProfile(ClientFinance client);
        Task<Response> AddDocument(List<DocumentINDH> documents, int ID);
        Task<Response> AddCandidat(CandidatINDHViewModel client);
        Task<PaginatedList<ClientFinance>> GetCandidats(string SearchPG, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, int PageSize);
        Task<Response> AddIndh(List<INDH> INDH, int ID, PlateformeGestionnaire pg);
    }
}
