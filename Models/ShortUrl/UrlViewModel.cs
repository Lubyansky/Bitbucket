using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bitbucket.Models.ShortUrl
{
    public class UrlViewModel
    {
        public UrlViewModel(IEnumerable<Url> urlList, Url url)
        {
            UrlList = urlList;
            Url = url;
        }
        public IEnumerable<Url> UrlList { get; }
        public Url Url { get; }
    }
}
