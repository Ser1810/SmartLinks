using Microsoft.AspNetCore.Http;

namespace SmartLinks.Interfaces
{
    public interface IRedirectProcessors
    {
        public HttpContext Context { get; set; }
        public bool Processor(IDictionary<string, object> args);
    }
}