using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Repository.Data;
using Repository.IRepositories;

namespace Repository.Repositories
{
#nullable disable
    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailRepository(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> SendEmailAsync(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Templates");
            string filePath = Path.Combine(uploads, "ForgotPassword.html");

            mailTo = await _dbContext.EmailEnvois.Where(d => d.Description == "NouvelleDemande").Select(e => e.Email).FirstOrDefaultAsync();
            //var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\Web\\wwwroot\\ForgotPassword.html"));
            //var filePath = $"{Directory.GetCurrentDirectory()}\\/\\Service\\Templates\\ForgotPassword.html";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[Nom]", "Badr").Replace("[Prenom]", "Moussaoui").Replace("[Lien]", Lien);

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse("PlateformeDesJeunes_AinSebaa@hotmail.com"),
                Subject = subject,
            };
            email.From.Add(new MailboxAddress("Plateforme des jeunes Ain sebaa", "PlateformeDesJeunes_AinSebaa@hotmail.com"));
            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();


            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("PlateformeDesJeunes_AinSebaa@hotmail.com", "Admin123@@");
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

            return true;
        }
        public async Task<bool> SendEmailAsyncConfirmEmail(string mailTo, string subject, string Lien, IList<IFormFile> attachments = null)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Templates");
            string filePath = Path.Combine(uploads, "ConfirmEmail.html");

            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[Lien]", Lien);

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse("PlateformeDesJeunes_AinSebaa@hotmail.com"),
                Subject = subject,
            };
            email.From.Add(new MailboxAddress("Plateforme des jeunes Ain sebaa", "PlateformeDesJeunes_AinSebaa@hotmail.com"));
            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();


            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("PlateformeDesJeunes_AinSebaa@hotmail.com", "Admin123@@");
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

            return true;
        }


        public async Task<bool> SendException(string subject, string Description, string Page)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Templates");
            string filePath = Path.Combine(uploads, "ExceptionTemplate.html");
            //var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\Web\\wwwroot\\Templates\\ExceptionTemplate.html"));
            //var filePath = $"{Directory.GetCurrentDirectory()}\\/\\Service\\Templates\\ForgotPassword.html";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[Page]", Page);
            mailText = mailText.Replace("[Exception]", Description);

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse("PlateformeDesJeunes_AinSebaa@hotmail.com"),
                Subject = subject,
            };
            email.From.Add(new MailboxAddress("Plateforme des jeunes Ain sebaa", "PlateformeDesJeunes_AinSebaa@hotmail.com"));
            email.To.Add(MailboxAddress.Parse("badredine_moussaoui@hotmail.com"));

            var builder = new BodyBuilder();

            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();


            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("PlateformeDesJeunes_AinSebaa@hotmail.com", "Admin123@@");
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

            return true;
        }
    }
}
