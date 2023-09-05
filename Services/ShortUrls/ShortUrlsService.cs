using ShortenUrl.Data;
using ShortenUrl.Models.ShortUrl;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ShortenUrl.Services.ShortUrls
{
    public class ShortUrlsService
    {
        private readonly ShortenUrlContext _context;

        public ShortUrlsService(ShortenUrlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Url>> GetByUserId(int UserId)
        {
            using ShortenUrlContext db = new();

            IEnumerable<Url> UrlList = await db.Urls
                .Where(x => x.UserId == UserId)
                .OrderBy(x => x.UrlId)
                .ToListAsync();

            return UrlList;
        }

        public async Task<Url> GetByToken(string Token)
        {
            Url Url = await _context.Urls
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

            Url.Token = builder.ToString();
            Url.UserId = UserId;
            Url.CreatedAt = DateTime.Now;

            await _context.Urls.AddAsync(Url);
            await _context.SaveChangesAsync();

        }

        public async Task IncrementClick(Url Url)
        {
            Url.Clicks++;
            _context.Entry(Url).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
