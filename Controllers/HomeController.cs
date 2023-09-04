using Bitbucket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace Bitbucket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            using BitbucketContext db = new();

            var currentUserId = await db.Users
                .Where(x => x.Email == User.Identity.Name)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();

            IEnumerable<Url> urlList = await db.Urls
                .Where(x => x.UserId == currentUserId)
                .ToListAsync();

            Url url = new();

            UrlViewModel viewModel = new(urlList, url);

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}