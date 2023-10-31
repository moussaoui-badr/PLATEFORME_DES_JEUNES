using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using System.Security.Claims;

namespace Repository.Repositories
{
#nullable disable
    public class FormationRepository : IFormationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FormationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Formation>> getFormations(int? pageNumber, ClaimsPrincipal claims, string Theme, string Animateur, int Duree, string SearchDate, string SearchDateO)
        {
            int pageSize = 10;

            var formations = _dbContext.Formations.AsQueryable();

            if (claims.IsInRole("HeureJoyeuse"))
            {
                formations = formations.Where(c => c.Animateur == "L\'HEURE JOYEUSE");
            }
            if (claims.IsInRole("Amideast"))
            {
                formations = formations.Where(c => c.Animateur == "Amideast");
            }
            if (Theme != null)
            {
                formations = formations.Where(c => c.Theme == Theme);
            }
            if (Animateur != null)
            {
                formations = formations.Where(c => c.Animateur == Animateur);
            }
            if (Duree != 0)
            {
                formations = formations.Where(c => c.DureeFormation == Duree);
            }
            if (SearchDate != null)
            {
                DateTime Date = Convert.ToDateTime(SearchDate);
                formations = formations.Where(p => p.DateFormation >= Date);
            }
            if (SearchDateO != null)
            {
                DateTime Date2 = Convert.ToDateTime(SearchDateO);
                formations = formations.Where(p => p.DateFormation >= Date2);
            }

            return await PaginatedList<Formation>.CreateAsync(formations, pageNumber ?? 1, pageSize);
        }


        public async Task<Formation> getFormation(int Id)
        {
            return await _dbContext.Formations.Where(i => i.FormationID == Id).Include(u => u.InscriptionFormation).ThenInclude(i => i.Client).FirstOrDefaultAsync();
        }

        public async Task AddFormation(Formation formation)
        {
            await _dbContext.Formations.AddAsync(formation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(Formation formation)
        {
            _dbContext.Update(formation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _dbContext.Formations.Remove(new Formation { FormationID = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedList<Client>> getCandidats(int FormationID, string Theme, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims)
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
            if (!string.IsNullOrEmpty(OrientationFilter))
            {
                clients = clients.Where(c => c.Oriente == (OrientationFilter == "Oui" ? true : false));
            }
            if (!string.IsNullOrEmpty(SearchCIN))
            {
                clients = clients.Where(c => c.CIN == SearchCIN);
            }
            if (!string.IsNullOrEmpty(SearchStatut))
            {
                clients = clients.Where(c => c.Statut == (SearchStatut == "0" ? Statut.PorteurDeProjet : Statut.ChercheurEmploit));
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

            clients = clients.Include(c => c.InscriptionFormation);

            int pageSize = 10;

            return await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize);

        }

        public async Task EditAffectation(int formationId, int clientId, bool valide)
        {
            if (valide)
            {
                await _dbContext.InscriptionFormation.AddAsync(new InscriptionFormation
                {
                    FormationID = formationId,
                    ClientID = clientId
                });
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var inscription = await _dbContext.InscriptionFormation.Where(c => c.ClientID == clientId && c.FormationID == formationId).FirstOrDefaultAsync();
                _dbContext.InscriptionFormation.Remove(inscription);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
