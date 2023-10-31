using Domain.Entities;
using Domain.Enums;
using Repository.Data;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class CandidatPublicRepository : ICandidatPublicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CandidatPublicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> AddCandidat(Client client)
        {
            try
            {
                if (client != null)
                {
                    await _dbContext.AddAsync(client);
                    await _dbContext.SaveChangesAsync();
                    return new Response
                    {
                        Success = true,
                        ID = client.ClientID.GetValueOrDefault(),
                    };
                }
                return new Response
                {
                    Success = false,
                    ID = 0,
                    Message = "Aucune valeur a insérer"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Success = false,
                    ID = 0,
                    Message = "Une erreur est survenu.",
                    Message2 = ex.Message.ToString()+"\n"+ex.InnerException != null ? ex.InnerException.Message : "",
                };
            }
        }

        public async Task<Response> AddDocument(List<Document> documents, int ID)
        {
            if (documents != null)
            {
                await _dbContext.Documents.AddRangeAsync(documents);
                await _dbContext.SaveChangesAsync();
            }

            return new Response { Success = true };
        }

        public async Task<Response> AddDiplome(List<Diplome> Diplome, int ID)
        {
            if (Diplome != null)
            {
                await _dbContext.Diplomes.AddRangeAsync(Diplome);
                await _dbContext.SaveChangesAsync();
            }

            return new Response { Success = true };
        }

    }
}
