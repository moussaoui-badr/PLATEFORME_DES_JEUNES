using Domain.Entities;
using Domain.Enums;

namespace Repository.IRepositories
{
    public interface ICandidatPublicRepository
    {
        Task<Response> AddCandidat(Client client);
        Task<Response> AddDocument(List<Document> documents, int ID);
        Task<Response> AddDiplome(List<Diplome> Diplome, int ID);
    }
}
