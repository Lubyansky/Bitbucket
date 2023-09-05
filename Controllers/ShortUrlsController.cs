using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShortenUrl.Models.ShortUrl;
using ShortenUrl.Services.ShortUrls;
using ShortenUrl.Services.Account;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortenUrl.Models;

namespace ShortenUrl.Controllers
{
    public class ShortUrlsController : Controller
    {
        private readonly ILogger<ShortUrlsController> _logger;
        private readonly ShortUrlsService _UrlsService;
        private readonly IGetUserService _UserService;

        public ShortUrlsController(ILogger<ShortUrlsController> logger, ShortUrlsService UrlsService, IGetUserService UserService)
        {
            _logger = logger;
            _UrlsService = UrlsService;
            _UserService = UserService;
        }

        [Authorize]
        public async Task<IActionResult> Index(int Page = 1, int PerPage = 10)
        {
            int currentUserId = await _UserService.GetUserId(User.Identity.Name);
            var UrlList = await _UrlsService.GetByUserId(currentUserId);

            PageViewModel PageViewModel = new(UrlList.Count(), Page, PerPage);

            UrlViewModel Model = new(UrlList, PageViewModel, new Url());

            @ViewData["Path"] = HttpContext.Request.Host;

            return View(Model);
        }

        public async Task<IActionResult> Shorten(Url Url)
        {
            int currentUserId = await _UserService.GetUserId(User.Identity.Name);

            await _UrlsService.ShortenUrl(currentUserId, Url);

            return RedirectToAction("Index", "ShortUrls");
        }

        public async Task<IActionResult> RedirectTo(string Path)
        {
            if (Path == null)
            {
                return NotFound();
            }

            var Url = await _UrlsService.GetByToken(Path);

            if (Url == null)
            {
                return NotFound();
            }

            await _UrlsService.IncrementClick(Url);

            return Redirect(Url.Original);
        }

    }
}