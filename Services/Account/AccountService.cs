using ShortenUrl.Data;
using ShortenUrl.Models.Account;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShortenUrl.Services.Account
{
    public class AccountService : IGetUserService
    {
        private readonly ShortenUrlContext _context;

        public AccountService(ShortenUrlContext context)
        {
            _context = context;
        }

        public async Task<int> GetUserId(string Username)
        {
            int currentUserId = await _context.Users
                 .Where(x => x.Email == Username)
                 .Select(x => x.UserId)
                 .FirstOrDefaultAsync();

            return currentUserId;
        }

        public async Task<Boolean> FindUser(BaseAuthModel Model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Model.Email);
            return user != null;
        }

        public async Task<Boolean> AuthenticateUser(BaseAuthModel Model) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Model.Email && u.Password == Model.Password);
            return user != null;
        }

        public async Task CreateUser(BaseAuthModel Model)
        {
            await _context.Users.AddAsync(new User { Email = Model.Email, Password = Model.Password });
            await _context.SaveChangesAsync();
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
