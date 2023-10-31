using Domain.Enums;
using Domain.Models;

namespace Service.IService
{
    public interface ICandidatPublicService
    {
        Task<Response> AddCandidat(CandidatViewModel client);
        Task<Response> AddDocument(List<DocumentModel> documents, int ID);
        Task<Response> AddDiplome(List<DiplomeModel> DiplomeModel, int ID);
    }
}
