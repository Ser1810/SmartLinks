using SmartLinks.Interfaces;

public class RedirectMiddleware(ISmartLinksService smartLinksService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var path = context.Request.Path.Value?.TrimStart('/');

            if (path == string.Empty)
            {
                var redirect = await smartLinksService.SearchRule(context);

                if (redirect != null)
                {
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
}