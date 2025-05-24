using SmartLinks;
using SmartLinks.Interfaces;

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<RedirectMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ISmartLinksService, SmartLinkService>();

var app = builder.Build();

app.UseMiddleware<RedirectMiddleware>();

app.Run();