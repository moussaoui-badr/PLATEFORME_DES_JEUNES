using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using System.Security.Claims;


namespace Repository.Repositories
{
#nullable disable
    public class CandidatRepository : ICandidatRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CandidatRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Client>> GetCandidatsO(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int PageSize)
        {
                var clients = _dbContext.Clients.Where(p => p.Public == false).AsQueryable();

                if (claims.IsInRole("HeureJoyeuse"))
                {
                    clients = clients.Where(c => c.Statut == Statut.PorteurDeProjet);
                }
                if (claims.IsInRole("Amideast"))
                {
                    clients = clients.Where(c => c.Statut == Statut.ChercheurEmploit);
                }
                if (!string.IsNullOrEmpty(SearchCIN))
                {
                    clients = clients.Where(c => c.CIN == SearchCIN);
                }
                if (!string.IsNullOrEmpty(OrientationFilter))
                {
                    clients = clients.Where(c => c.Orientation == (OrientationFilter == "Orienté" ? Orientation.Orienté : OrientationFilter == "NonOrienté" ? Orientation.NonOrienté : OrientationFilter == "Refusé" ? Orientation.Refusé : Orientation.Financé));
                }
                if (!string.IsNullOrEmpty(SearchStatut))
                {
                    if (claims.IsInRole("Admin") || claims.IsInRole("Gestionnaire"))
                    {
                        clients = clients.Where(c => c.Statut == (SearchStatut == "0" ? Statut.PorteurDeProjet : Statut.ChercheurEmploit));

                    }
                }
                if (!string.IsNullOrEmpty(SearchNom))
                {
                    clients = clients.Where(n => n.Nom.Contains(SearchNom));
                }
                if (!string.IsNullOrEmpty(SearchPrenom))
                {
                    clients = clients.Where(p => p.Prenom.Contains(SearchPrenom));
                }

                if (SearchDate != null)
                {
                    DateTime oDate = Convert.ToDateTime(SearchDate);
                    clients = clients.Where(p => p.DateAderation >= oDate);
                }
                if (SearchDateO != null)
                {
                    DateTime oDateO = Convert.ToDateTime(SearchDateO);
                    clients = clients.Where(p => p.DateAderation <= oDateO);
                }

                int pageSize = PageSize;

                return await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize);
            
        }

        public async Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims, int? PageSize)
        {
            var clients = _dbContext.Clients.Where(p => p.Public == false).AsQueryable();

            if (claims.IsInRole("HeureJoyeuse"))
            {
                clients = clients.Where(c => c.Statut == Statut.PorteurDeProjet);
            }
            if (claims.IsInRole("Amideast"))
            {
                clients = clients.Where(c => c.Statut == Statut.ChercheurEmploit);
            }
            if (!string.IsNullOrEmpty(SearchCIN))
            {
                clients = clients.Where(c => c.CIN == SearchCIN);
            }
            if (!string.IsNullOrEmpty(OrientationFilter))
            {
                clients = clients.Where(c => c.Orientation == (OrientationFilter == "Orienté" ? Orientation.Orienté : OrientationFilter == "NonOrienté" ? Orientation.NonOrienté : OrientationFilter == "Refusé" ? Orientation.Refusé : Orientation.Financé));
            }
            if (!string.IsNullOrEmpty(SearchStatut))
            {
                if (claims.IsInRole("Admin") || claims.IsInRole("Gestionnaire"))
                {
                    clients = clients.Where(c => c.Statut == (SearchStatut == "0" ? Statut.PorteurDeProjet : Statut.ChercheurEmploit));

                }
            }
            if (!string.IsNullOrEmpty(SearchNom))
            {
                clients = clients.Where(n => n.Nom.Contains(SearchNom));
            }
            if (!string.IsNullOrEmpty(SearchPrenom))
            {
                clients = clients.Where(p => p.Prenom.Contains(SearchPrenom));
            }

            if (SearchDate != null)
            {
                DateTime oDate = Convert.ToDateTime(SearchDate);
                clients = clients.Where(p => p.DateAderation >= oDate);
            }
            if (SearchDateO != null)
            {
                DateTime oDateO = Convert.ToDateTime(SearchDateO);
                clients = clients.Where(p => p.DateAderation <= oDateO);
            }


            var result = await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, PageSize ?? 15);
            return result;
        }

        public async Task<Client> GetCandidat(int ID)
        {
            return await _dbContext.Clients.Where(i => i.ClientID == ID)
                .Include(s => s.Diplomes)
                .Include(s => s.Documents)
                .Include(i => i.InscriptionFormation)
                .ThenInclude(f => f.Formation)
                .AsNoTracking()
                .FirstOrDefaultAsync();
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
                        ID = client.ClientID.GetValueOrDefault()
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
                    Message2 = ex.Message.ToString(),
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

        public async Task<Response> EditCandidat(Client client)
        {
            _dbContext.Clients.Update(client);
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true, ID = client.ClientID.GetValueOrDefault() };
        }

        public async Task<Response> EditDiplome(List<Diplome> diplome)
        {
            _dbContext.Diplomes.UpdateRange(diplome);
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }

        public async Task<Response> EditDocument(List<Document> documents)
        {
            _dbContext.Documents.UpdateRange(documents);
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }

        public async Task<Response> Delete(int id)
        {
            var documents = await _dbContext.Documents.Where(i => i.ClientID == id).ToListAsync();
            _dbContext.Documents.RemoveRange(documents);

            var diplomes = await _dbContext.Diplomes.Where(i => i.ClientID == id).ToListAsync();
            _dbContext.Diplomes.RemoveRange(diplomes);

            var inscriptionFormation = await _dbContext.InscriptionFormation.Where(i => i.ClientID == id).ToListAsync();
            _dbContext.InscriptionFormation.RemoveRange(inscriptionFormation);

            _dbContext.Clients.Remove(new Client
            {
                ClientID = id
            });
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }
    }
}
