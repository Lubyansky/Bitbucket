using Bitbucket.Models.Account;
using Bitbucket.Models.ShortUrl;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bitbucket.Services.Account
{
    public interface IGetUserService
    {
        Task<int> GetUserId(string Username);
        Task<Boolean> FindUser(BaseAuthModel Model);

    }
}
