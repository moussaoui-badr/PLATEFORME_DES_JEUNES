using Microsoft.AspNetCore.Http;
using Repository.IRepositories;
using Service.IService;

namespace Service.Service
{
#nullable disable
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task<bool> SendEmailAsync(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null)
        {
            return await _emailRepository.SendEmailAsync(mailTo, subject, Lien, attachments);
        }

        public async Task<bool> SendEmailAsyncConfirmEmail(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null)
        {
            return await _emailRepository.SendEmailAsyncConfirmEmail(mailTo, subject, Lien, attachments);
        }
    }
}
