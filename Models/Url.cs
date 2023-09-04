using System;
using System.Collections.Generic;

namespace Bitbucket.Models;

public partial class Url
{
    public int UrlId { get; set; }

    public string Original { get; set; } = null!;

    public string Token { get; set; } = null!;

    public int Clicks { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
