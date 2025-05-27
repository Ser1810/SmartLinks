using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SmartLinks.Interfaces;
using System.Net.Http;

namespace SmartLinks.Processors
{
    public class TimeProcessor : IRedirectProcessors
    {
        public HttpContext Context { get; set; }

        private readonly string _field = "date";
        public TimeProcessor() { }

        public bool Processor(IDictionary<string, object> args)
        {
            object value = null;

            if (args == null || !args.TryGetValue(_field, out value))
            {
                return false;
            }

            var date = value.ToString() ?? string.Empty;
            var nowUtc = DateTime.UtcNow;

            var dateRange = JsonConvert.DeserializeObject<DateRange>(date);

            if (dateRange != null && dateRange.Begin < nowUtc && dateRange.End > nowUtc)
            {
                return true;
            }

            return false;
        }
    }

    [Serializable]
    internal class DateRange
    {
        public DateTime? Begin { get; set; }

        public DateTime? End { get; set; }
    }
}