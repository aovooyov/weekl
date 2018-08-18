using System.Configuration;
using Ninject.Modules;
using Ninject.Parameters;
using Weekl.Core.Repository.Feed;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Core.Service;
using Weekl.Core.Service.Abstract;

namespace Weekl.Service.Worker
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            var connectionString = new ConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["Weekl"].ConnectionString);

            Bind<ISourceRepository>().To<SourceRepository>().WithParameter(connectionString);
            Bind<IChannelRepository>().To<ChannelRepository>().WithParameter(connectionString);
            Bind<IArticleRepository>().To<ArticleRepository>().WithParameter(connectionString);
            Bind<IRssService>().To<RssService>();
            Bind<IWorker>().To<Worker>();
        }
    }
}