using System.Web.Http;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Weekl.Api.Infrastructure;

[assembly: OwinStartup(typeof(Weekl.Api.Startup))]

namespace Weekl.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var kernel = new StandardKernel();
            
            DependenciesConfig.Register(kernel);
            FiltersConfig.Register(config);
            CorsConfig.Register(config);
            ApiConfig.Register(config);

            app.UseNinjectMiddleware(() => kernel);
            app.UseNinjectWebApi(config);
            app.UseWebApi(config);
        }
    }
}
