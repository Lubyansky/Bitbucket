using Bitbucket.Models.ShortUrl;

namespace Bitbucket.Models.Account;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
