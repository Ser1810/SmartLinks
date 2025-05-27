using SmartLinks;
using SmartLinks.Interfaces;

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<RedirectMiddleware>();

builder.Services.AddScoped<ISmartLinksService, SmartLinkService>();

var app = builder.Build();

app.UseMiddleware<RedirectMiddleware>();

app.Run();