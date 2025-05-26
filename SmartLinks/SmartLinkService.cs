using DB;
using Microsoft.AspNetCore.Http;
using Model;
using Newtonsoft.Json;
using Ninject;
using SmartLinks.Interfaces;
using System.Data;
using System.Reflection;

namespace SmartLinks
{
    public class SmartLinkService : ISmartLinksService
    {
        public string TableName = "Rule";
        public async Task<string> SearchRule(HttpContext context)
        {            
            var sql = $"select \"RuleDll\", \"RedirectTo\", \"Args\" from \"{TableName}\" where \"IsActive\" = true order by \"Order\"";

            
            var redirectRules = SqlExecutor.ExecuteSql<RuleRedirect>(sql, GetRuleRedirect);

            if (redirectRules == null || !redirectRules.Any())
            {
                return null;
            }

            var processorTypes = GetProcessors();
            var actualRules = from r in redirectRules where processorTypes.Select(s => s.Name).Contains(r.RuleDll) select r;

            IKernel ninjectKernel = new StandardKernel();

            foreach (var actualRule in actualRules)
            {
                var type = processorTypes.FirstOrDefault(f => f.Name == actualRule.RuleDll);

                ninjectKernel.Rebind(typeof(IRedirectProcessors)).To(type);

                var processor = ninjectKernel.Get<IRedirectProcessors>();
                processor.Context = context;

                IDictionary<string, object> args = new Dictionary<string, object>();

                try
                {
                    args = JsonConvert.DeserializeObject<IDictionary<string, object>>(actualRule.Args);
                }
                catch
                {
                    Console.WriteLine($"При десериализации {actualRule.Args} произошла ошибка");
                }

                if (processor.Processor(args))
                {
                    return actualRule.RedirectTo;
                }
            }

            return null;
        }

        protected List<Type> GetProcessors()
        {
            var rootPath = Assembly.GetExecutingAssembly().Location;
            var dirRootPath = Path.GetDirectoryName(rootPath);
            var mask = "Processors.*.dll";
            var files = Directory.EnumerateFiles(dirRootPath, mask, SearchOption.TopDirectoryOnly);

            var processors = new List<Type>();

            foreach (var file in files)
            {
                var asm = Assembly.LoadFrom(file);
                var validatorType = asm.ExportedTypes.SingleOrDefault();
                if (validatorType != null)
                {
                    processors.Add(validatorType);
                }
            }

            return processors;
        }

        protected RuleRedirect GetRuleRedirect(IDataReader reader)
        {
            var rule = new RuleRedirect
            {
                RuleDll = reader[0].ToString(),
                RedirectTo = reader[1].ToString(),
                Args = reader[2].ToString()
            };
            return rule;
        }
    }
}