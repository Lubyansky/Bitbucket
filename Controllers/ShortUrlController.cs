using Bitbucket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Text;


namespace Bitbucket.Controllers
{
    public class ShortUrlController : Controller
    {
        private readonly ILogger<ShortUrlController> _logger;

        public ShortUrlController(ILogger<ShortUrlController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Shorten(Url url)
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
            var currentUserId = await db.Users
                .Where(x => x.Email == User.Identity.Name)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();

            url.Token = builder.ToString();
            url.UserId = currentUserId;

            await db.Urls.AddAsync(url);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}