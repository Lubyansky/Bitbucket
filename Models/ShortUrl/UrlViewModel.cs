using ShortenUrl.Models;

namespace ShortenUrl.Models.ShortUrl
{
    public class UrlViewModel
    {
        public UrlViewModel(IEnumerable<Url> _UrlList, PageViewModel _PageViewModel, Url _Url)
        {
            UrlList = _UrlList;
            PageViewModel = _PageViewModel;
            Url = _Url;
        }
        public IEnumerable<Url> UrlList { get; }
        public PageViewModel PageViewModel { get; set; }
        public Url Url { get; }
    }
}
