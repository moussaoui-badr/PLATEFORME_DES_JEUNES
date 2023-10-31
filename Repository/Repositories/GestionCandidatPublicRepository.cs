


using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using System.Security.Claims;

namespace Repository.Repositories
{
#nullable disable
    public class GestionCandidatPublicRepository : IGestionCandidatPublicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GestionCandidatPublicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Client> GetCandidat(int Id)
        {
            return await _dbContext.Clients
                .Include(d => d.Diplomes)
                .Include(f => f.Documents)
                .Where(p => p.Public == true)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ClientID == Id);
        }
        public async Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims)
        {
            var clients = _dbContext.Clients.Where(p => p.Public == true).AsQueryable();

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
                clients = clients.Where(c => c.Oriente == (OrientationFilter == "Oui" ? true : false));
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

            int pageSize = 10;

            return await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize);

        }

        public async Task Valider(int Id)
        {
            var candidat = await _dbContext.Clients.FirstOrDefaultAsync(i => i.ClientID == Id);
            candidat.Public = false;
            _dbContext.Update(candidat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var documents = await _dbContext.Documents.Where(i => i.ClientID == Id).ToListAsync();
            var diplomes = await _dbContext.Diplomes.Where(i => i.ClientID == Id).ToListAsync();

            if (documents != null)
            {
                _dbContext.RemoveRange(documents);
            }
            if (diplomes != null)
            {
                _dbContext.RemoveRange(diplomes);
            }
            _dbContext.Clients.Remove(new Client { ClientID = Id });
            await _dbContext.SaveChangesAsync();
        }
    }
}
