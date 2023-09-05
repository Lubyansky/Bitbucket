using Bitbucket.Data;
using Bitbucket.Models.Account;
using Bitbucket.Models.ShortUrl;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Bitbucket.Services.ShortUrls
{
    public class ShortUrlsService
    {
        public async Task<IEnumerable<Url>> GetByUserId(int UserId)
        {
            using BitbucketContext db = new();

            IEnumerable<Url> UrlList = await db.Urls
                .Where(x => x.UserId == UserId)
                .OrderBy(x => x.UrlId)
                .ToListAsync();

            return UrlList;
        }

        public async Task<Url> GetByToken(string Token)
        {
            using BitbucketContext db = new();

            Url Url = await db.Urls
               .Where(u => u.Token == Token)
               .FirstOrDefaultAsync();

            return Url;
        }

        public async Task ShortenUrl(int UserId, Url Url)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => builder.Append(e));

            using BitbucketContext db = new();

            Url.Token = builder.ToString();
            Url.UserId = UserId;
            Url.CreatedAt = DateTime.Now;

            await db.Urls.AddAsync(Url);
            await db.SaveChangesAsync();

        }

        public async Task IncrementClick(Url Url)
        {
            using BitbucketContext db = new();
            Url.Clicks++;
            db.Entry(Url).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

    }
}
