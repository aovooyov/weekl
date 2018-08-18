using System.Configuration;
using Ninject;
using Ninject.Parameters;
using Ninject.Web.Common;
using Weekl.Core.Repository.Feed;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Api.Infrastructure
{
    public static class DependenciesConfig
    {
        public static void Register(IKernel kernel)
        {
            var connectionString = new ConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["Weekl"].ConnectionString);
            
            kernel.Bind<ISourceRepository>().To<SourceRepository>().InRequestScope().WithParameter(connectionString);
            kernel.Bind<IChannelRepository>().To<ChannelRepository>().InRequestScope().WithParameter(connectionString);
            kernel.Bind<IArticleRepository>().To<ArticleRepository>().InRequestScope().WithParameter(connectionString);
        }
    }
}