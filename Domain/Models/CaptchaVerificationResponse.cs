namespace Domain.Models
{
    public class CaptchaVerificationResponse
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
