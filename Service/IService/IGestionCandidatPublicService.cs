using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Service.IService
{
    public interface IGestionCandidatPublicService
    {
        Task<FileStreamResult> PrintPdf(int id);
        Task<Client> GetCandidat(int Id);
        Task Delete(int Id);
        Task Valider(int Id);
        Task<PaginatedList<Client>> GetCandidats(string SearchNom, string SearchPrenom, string SearchDate, string SearchDateO, int? pageNumber, string SearchCIN, string SearchStatut, string OrientationFilter, ClaimsPrincipal claims);
    }
}
