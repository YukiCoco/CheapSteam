using CheapSteam.UI.Data;
using ChpStmScraper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using CheapSteam.UI.Services;

var builder = WebApplication.CreateBuilder(args);
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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Console.WriteLine($"访问 http://127.0.0.1:{Configuration.ListenPort} 进入程序界面");

app.Run();