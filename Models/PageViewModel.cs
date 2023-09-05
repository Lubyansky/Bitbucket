namespace ShortenUrl.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int _Count, int _PageNumber, int _PageSize)
        {
            PageNumber = _PageNumber;
            TotalPages = (int)Math.Ceiling(_Count / (double)_PageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
