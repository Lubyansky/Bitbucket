using ShortenUrl.Models.Account;

namespace ShortenUrl.Services.Account
{
    public interface IGetUserService
    {
        Task<int> GetUserId(string Username);
        Task<Boolean> FindUser(BaseAuthModel Model);

    }
}
