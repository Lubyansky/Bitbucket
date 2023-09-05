using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitbucket.Models.ShortUrl;
using Bitbucket.Services.ShortUrls;
using Bitbucket.Services.Account;

namespace Bitbucket.Controllers
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
        public async Task<IActionResult> Index()
        {
            int currentUserId = await _UserService.GetUserId(User.Identity.Name);

            var UrlList = await _UrlsService.GetByUserId(currentUserId);

            UrlViewModel Model = new(UrlList, new Url());

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