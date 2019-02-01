using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NLog;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Core.Service.Abstract;

namespace Weekl.Service.Worker
{
    public class Worker : IWorker
    {
        private readonly IRssService _rssService;
        private readonly ISourceRepository _sourceRepository;
        private readonly ILogger _logger;

        public Worker(
            IRssService rssService,
            ISourceRepository sourceRepository)
        {
            _rssService = rssService;
            _sourceRepository = sourceRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task SyncFeed()
        {
            while (true)
            {
                _logger.Info("[Sync Feed]");

                var count = await _rssService.SyncFeed();

                _logger.Info($"[Sync Feed] Total Articles Count: {count}");

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        public async Task SyncFeed(int sourceId)
        {
            while (true)
            {
                var source = _sourceRepository.Get(sourceId);
                if (source == null)
                {
                    return;
                }

                _logger.Info($"[{source.Unique}] Sync Feed: {source.Name}");

                var count = await _rssService.SyncFeed(source);

                _logger.Info($"[{source.Unique}] Articles: {count}");

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        public async Task SyncFeedAsync()
        {
            while (true)
            {
                _logger.Info("[SyncFeedAsync]: start");

                var sources = _sourceRepository.List();
                var tasks = sources
                    .Select(source => _rssService.SyncFeed(source))
                    .ToList();

                while (tasks.Count > 0)
                {
                    var completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                    tasks.Remove(completed);
                }

                _logger.Info("[SyncFeedAsync]: done");

                _rssService.Clean();

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
    }
}