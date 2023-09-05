using ShortenUrl.Models.Account;

namespace ShortenUrl.Models.ShortUrl;

public partial class Url
{
    public int UrlId { get; set; }

    public string Original { get; set; } = null!;

    public string Token { get; set; } = null!;

    public int Clicks { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
