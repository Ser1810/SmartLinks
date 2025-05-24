using Microsoft.AspNetCore.Http;
using SmartLinks.Interfaces;
using UAParser;

namespace SmartLinks.Processors
{
    public class BrowserProcessor : IRedirectProcessors
    {
        public HttpContext Context { get; set; }

        private readonly string _field = "browser";
        private Parser _parser = Parser.GetDefault();
        public BrowserProcessor() { }

        public bool Processor(IDictionary<string, object> args)
        {
            var userAgent = Context?.Request?.Headers["User-Agent"].ToString() ?? string.Empty;

            ClientInfo clientInfo = _parser.Parse(userAgent);
            var browser = clientInfo?.UA.Family.ToLower();

            object value = null;

            if (args == null || !args.TryGetValue(_field, out value))
            {
                return false;
            }

            if (value != null && browser.Contains(value.ToString()?.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}