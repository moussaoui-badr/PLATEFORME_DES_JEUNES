namespace Service.IService
{
    public interface IRecaptchaService
    {
        Task<bool> IsCaptchaValid(string token);
    }
}
