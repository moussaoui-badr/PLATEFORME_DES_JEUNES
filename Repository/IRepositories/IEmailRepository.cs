using Microsoft.AspNetCore.Http;

namespace Repository.IRepositories
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null);
        Task<bool> SendEmailAsyncConfirmEmail(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null);
    }
}
