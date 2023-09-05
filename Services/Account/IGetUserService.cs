using Bitbucket.Models.Account;

namespace Bitbucket.Services.Account
{
    public interface IGetUserService
    {
        Task<int> GetUserId(string Username);
        Task<Boolean> FindUser(BaseAuthModel Model);

    }
}
