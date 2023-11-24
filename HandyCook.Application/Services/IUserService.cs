namespace HandyCook.Application.Services
{
    public interface IUserService
    {
        Task<string> GetCurrentUserIdAsync();
    }
}
