using Microsoft.AspNetCore.Http;

namespace Service.IService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null);
        Task<bool> SendEmailAsyncConfirmEmail(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null);
    }
}
