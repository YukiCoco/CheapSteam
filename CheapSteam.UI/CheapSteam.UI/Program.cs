using CheapSteam.UI.Data;
using ChpStmScraper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using CheapSteam.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CheamSteam.UI.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
builder.WebHost.ConfigureKestrel(option => option.ListenLocalhost(Configuration.ListenPort));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
if (!string.IsNullOrEmpty(Configuration.ProxyUrl))
{
    builder.Services.AddSingleton<ChpStmScraper.Services.HttpService>(new ChpStmScraper.Services.HttpService(Configuration.ProxyUrl));
}
else
{
    builder.Services.AddSingleton<ChpStmScraper.Services.HttpService>();
}
builder.Services.AddDbContext<ScraperDbContext>(ServiceLifetime.Singleton);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite(Configuration.ConnectionString));
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlite(Configuration.ConnectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSingleton<ScraperService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if (!File.Exists("ChpStmScraper.db"))
{
    File.Copy("ChpStmScraper.Template.db", "ChpStmScraper.db");
    Console.WriteLine($"访问 http://127.0.0.1:{Configuration.ListenPort}/settings 进入程序配置界面");
} else Console.WriteLine($"访问 http://127.0.0.1:{Configuration.ListenPort} 进入程序界面");
app.UseAuthorization();

app.Run();