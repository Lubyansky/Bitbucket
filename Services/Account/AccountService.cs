using Bitbucket.Data;
using Bitbucket.Models.Account;
using Bitbucket.Models.ShortUrl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Bitbucket.Services.Account
{
    public class AccountService : IGetUserService
    {
        public async Task<int> GetUserId(string Username)
        {
            using BitbucketContext db = new();

            int currentUserId = await db.Users
                 .Where(x => x.Email == Username)
                 .Select(x => x.UserId)
                 .FirstOrDefaultAsync();

            return currentUserId;
        }

        public async Task<Boolean> FindUser(BaseAuthModel Model)
        {
            using BitbucketContext db = new();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Model.Email);
            return user != null;
        }

        public async Task<Boolean> AuthenticateUser(BaseAuthModel Model) {
            using BitbucketContext db = new();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Model.Email && u.Password == Model.Password);
            return user != null;
        }

        public async Task CreateUser(BaseAuthModel Model)
        {
            using BitbucketContext db = new();
            await db.Users.AddAsync(new User { Email = Model.Email, Password = Model.Password });
            await db.SaveChangesAsync();
        }

        public ClaimsIdentity SetClaimIdentity(BaseAuthModel Model)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,  Model.Email)
            };
            return new ClaimsIdentity(Claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

    }
}
