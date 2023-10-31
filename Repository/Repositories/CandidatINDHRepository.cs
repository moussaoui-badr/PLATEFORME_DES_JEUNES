using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using System.Data;

namespace Repository.Repositories
{
#nullable disable
    public class CandidatINDHRepository : ICandidatINDHRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CandidatINDHRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<ClientFinance>> GetCandidats(string SearchPG, string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, int PageSize)
        {
            var clients = _dbContext.ClientFinances.Include(i => i.INDHS).AsQueryable();

            if (!string.IsNullOrEmpty(SearchCIN))
            {
                clients = clients.Where(c => c.CIN == SearchCIN);
            }

            if (!string.IsNullOrEmpty(SearchPG))
            {
                clients = clients.Where(c => c.PlateformeGestionnaire == (SearchPG == "AinSebaa" ? PlateformeGestionnaire.AinSebaa : SearchPG == "RocheNoir" ? PlateformeGestionnaire.RocheNoir : PlateformeGestionnaire.HayMohammadi));
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

            return await PaginatedList<ClientFinance>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<ClientFinance> GetCandidat(int ID)
        {
            return await _dbContext.ClientFinances.Where(i => i.ClientFinanceID == ID)
                .Include(s => s.Documents)
                .Include(i => i.INDHS)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Response> AddCandidat(ClientFinance client)
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
                        ID = client.ClientFinanceID
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

        public async Task<Response> AddDocument(List<DocumentINDH> documents)
        {
            if (documents != null)
            {
                await _dbContext.DocumentINDHS.AddRangeAsync(documents);
                await _dbContext.SaveChangesAsync();
            }

            return new Response { Success = true };
        }


        public async Task<Response> EditCandidat(ClientFinance client)
        {
            client.Modified = DateTime.Now;
            _dbContext.ClientFinances.Update(client);
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true, ID = client.ClientFinanceID };
        }

        public async Task<Response> EditINDH(List<INDH> INDHs, PlateformeGestionnaire pg)
        {
            try
            {
                var dateNow = DateTime.Now;
                foreach (var item in INDHs)
                {
                    item.Modified = dateNow;
                    item.PlateformeGestionnaire = pg;
                    _dbContext.INDHS.Update(item);
                }

                await _dbContext.SaveChangesAsync();

                return new Response { Success = true, ID = INDHs.FirstOrDefault().ClientFinanceID.Value };
            }
            catch (Exception) { return new Response { Success = true, ID = 0 }; }

        }

        public async Task<Response> EditDocument(List<DocumentINDH> documents)
        {
            _dbContext.DocumentINDHS.UpdateRange(documents);
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }

        public async Task<Response> Delete(int id)
        {
            var documents = await _dbContext.DocumentINDHS.Where(i => i.ClientID == id).ToListAsync();
            _dbContext.DocumentINDHS.RemoveRange(documents);

            var indhs = await _dbContext.INDHS.Where(i => i.ClientFinanceID == id).ToListAsync();
            _dbContext.INDHS.RemoveRange(indhs);

            _dbContext.ClientFinances.Remove(new ClientFinance
            {
                ClientFinanceID = id
            });
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }
        public async Task<Response> AddIndh(List<INDH> INDH, int ID, PlateformeGestionnaire pg)
        {
            var dateNow = DateTime.Now;
            foreach (INDH indh in INDH)
            {
                await _dbContext.INDHS.AddAsync(new INDH
                {
                    Materiel = indh.Materiel,
                    PartIndh = indh.PartIndh,
                    ApportEnDhs = indh.ApportEnDhs,
                    ApportEnAmenagement = indh.ApportEnAmenagement,
                    Autre = indh.Autre,
                    MontantAcquisition = indh.MontantAcquisition,
                    Etat = indh.Etat,
                    Created = dateNow,
                    ClientFinanceID = ID,
                    PlateformeGestionnaire = pg,
                });
            }

            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }
        public async Task<Response> EditIndh(List<INDH> INDH, PlateformeGestionnaire pg)
        {
            var dateNow = DateTime.Now;
            foreach (INDH indh in INDH)
            {
                _dbContext.INDHS.Update(new INDH
                {
                    Created = indh.Created,
                    Materiel = indh.Materiel,
                    PartIndh = indh.PartIndh,
                    ApportEnDhs = indh.ApportEnDhs,
                    ApportEnAmenagement = indh.ApportEnAmenagement,
                    Autre = indh.Autre,
                    MontantAcquisition = indh.MontantAcquisition,
                    Etat = indh.Etat,
                    Modified = dateNow,
                    ClientFinanceID = indh.ClientFinanceID,
                    PlateformeGestionnaire = pg,
                    INDHID = indh.INDHID
                });
            }
            await _dbContext.SaveChangesAsync();
            return new Response { Success = true };
        }
        public async Task DeleteIndh(int ID)
        {
            _dbContext.Remove(new INDH
            {
                INDHID = ID
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<INDH_FiltreModel>> FiltreMaterielIndh(int? EtatMateriel, int? ApportPersonnel)
        {
            var materielIndh = _dbContext.INDHS.Where(c => c.ClientFinanceID != null).Include(c => c.ClientFinance).AsQueryable();

            if (EtatMateriel != null)
            {
                materielIndh = materielIndh.Where(c => c.Etat == (EtatMateriel == 1 ? Etat.EnCoursAcquisition : EtatMateriel == 0 ? Etat.Acquis : Etat.Amenagement));
            }

            var result = await materielIndh.ToListAsync();

            var groupBy = result.GroupBy(c => c.ClientFinanceID).Select(group => new INDH_FiltreModel
            {
                ClientFinanceID = group.Key,
                Nom = group.First().ClientFinance.Nom,
                Prenom = group.First().ClientFinance.Prenom,
                CIN = group.First().ClientFinance.CIN,
                PlateformeGestionnaire = group.First().ClientFinance.PlateformeGestionnaire.ToString(),
                ImageURL = group.First().ClientFinance.ImageUrl,
                ApportPersonnel = group.First().ClientFinance.ApportPersonnelConfirme,
                MontantProjet = group.First().ClientFinance.MontantProjet,
                MontantApportPersonnel = group.First().ClientFinance.MontantApportPersonnel,
                MontantINDH = group.First().ClientFinance.MontantINDH,
                INDHS = group.ToList() // Liste des INDH associés à ce groupe
            });

            if (ApportPersonnel != null)
            {
                groupBy = groupBy.Where(c => c.ApportPersonnel == (ApportPersonnel.Value == 1 ? true : false));
            }

            return groupBy;
        }
    }
}
