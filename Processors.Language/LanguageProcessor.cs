using Microsoft.AspNetCore.Http;
using SmartLinks.Interfaces;

namespace SmartLinks.Processors
{
    public class LanguageProcessor : IRedirectProcessors
    {
        public HttpContext Context { get; set; }

        private readonly string _field = "language";
        public LanguageProcessor() { }

        public bool Processor(IDictionary<string, object> args)
        {
            var acceptLanguage = Context?.Request?.Headers["Accept-Language"].ToString().ToLower() ?? string.Empty;
            object value = null;

            if (args == null || !args.TryGetValue(_field, out value))
            {
                return false;
            }

            var language = value.ToString() ?? string.Empty;

            if (language == "*" || acceptLanguage.Contains(value.ToString()?.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}