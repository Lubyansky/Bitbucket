using Bitbucket.Data;
using Bitbucket.Models.ShortUrl;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Bitbucket.Services.ShortUrls
{
    public class ShortUrlsService
    {
        public async Task<UrlViewModel> GetShortUrls(int UserId)
        {
            using BitbucketContext db = new();

            IEnumerable<Url> UrlList = await db.Urls
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            Url Url = new();

            return new UrlViewModel(UrlList, Url);
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

            await db.Urls.AddAsync(Url);
            await db.SaveChangesAsync();

        }
    }
}
