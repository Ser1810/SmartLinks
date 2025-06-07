using Newtonsoft.Json;
using SmartLinks.Interfaces;
using System.Text;

public class RedirectMiddleware(ISmartLinksService smartLinksService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var path = context.Request.Path.Value.TrimStart('/');

            if (path == string.Empty)
            {
                var redirect = await smartLinksService.SearchRule(context);

                if (redirect != null)
                {
                    await AddHistory(context, redirect);
                    context.Response.Redirect(redirect);
                }
                else
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);            
        }
    }

    public async Task AddHistory(HttpContext context, string redirect)
    {
        try
        {
            var client = new HttpClient();
            var requestBody = new
            {
                URL = context.Request.Path.Value,
                RedirectURL = redirect,
                DateTime = DateTime.Now,
                Headers = System.Text.Json.JsonSerializer.Serialize(context.Request.Headers)
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string host = Environment.GetEnvironmentVariable("HISTORYAPI_HOST");

            var response = await client.PostAsync($"http://{host}:5013/History/add", content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}