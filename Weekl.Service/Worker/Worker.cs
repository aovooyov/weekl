using NLog;
using Weekl.Core.Service.Abstract;

namespace Weekl.Service.Worker
{
    public class Worker : IWorker
    {
        private readonly IRssService _rssService;
        private readonly ILogger _logger;

        public Worker(
            IRssService rssService)
        {
            _rssService = rssService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void SyncFeed()
        {
            _logger.Info("Sync Feed");

            var time = _rssService.SyncFeed();

            _logger.Info($"Sync Feed Time {time}");
        }
    }
}