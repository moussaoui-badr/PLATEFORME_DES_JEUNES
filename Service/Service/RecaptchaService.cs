using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service.IService;

namespace Service.Service
{
    public class RecaptchaService : IRecaptchaService
    {
        private CaptchaSettings captchaSettings;
        private ILogger<RecaptchaService> logger;

        public string ClientKey => captchaSettings.ClientKey;

        public RecaptchaService(IOptions<CaptchaSettings> captchaSettings, ILogger<RecaptchaService> logger)
        {
            this.captchaSettings = captchaSettings.Value;
            this.logger = logger;
        }

        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;

            var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsync($"{googleVerificationUrl}?secret={captchaSettings.ServerKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.success;
            }
            catch (Exception e)
            {
                // fail gracefully, but log
                logger.LogError("Failed to process captcha validation", e);
            }

            return result;
        }
    }
}
