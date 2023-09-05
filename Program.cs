using ShortenUrl.Data;
using ShortenUrl.Services.Account;
using ShortenUrl.Services.ShortUrls;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ShortUrlsService, ShortUrlsService>();
builder.Services.AddScoped<AccountService, AccountService>();
builder.Services.AddScoped<IGetUserService, AccountService>();
builder.Services.AddDbContext<ShortenUrlContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(name: "redirection",
                pattern: "{path:required}",
                defaults: new { controller = "ShortUrls", action = "RedirectTo" });

app.Run();
