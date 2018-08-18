using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Weekl.Api.Infrastructure.Configuration;

namespace Weekl.Api.Infrastructure
{
    public static class CorsConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute(string.Join(",", Origins), "*", "*")
            {
                SupportsCredentials = true
            });
        }

        public static string[] Origins
        {
            get
            {
                var corsConfig = (CorsSection)ConfigurationManager.GetSection("corsConfig");
                var cors = corsConfig.Cors.OfType<CorsType>().AsQueryable();
                var origins = cors.Select(c => c.Origin).ToArray();

                return origins;
            }
        }
    }
}