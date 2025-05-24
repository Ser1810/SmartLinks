using Microsoft.AspNetCore.Http;

namespace SmartLinks.Interfaces
{
    public interface ISmartLinksService
    {
        Task<string?> SearchRule(HttpContext context);
    }
}