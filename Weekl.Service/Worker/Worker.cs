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

        public async Task SyncFeedAsync()
        {
            while (true)
            {
                _logger.Info($"[Sync Feed] {DateTime.Now}: Start");

                try
                {
                    var total = await _rssService.SyncFeedAsync();
                    _logger.Info($"[Sync Feed] {DateTime.Now}: Total Articles Count: {total}");
                }
                catch (Exception e)
                {
                    _logger.Info($"[SyncFeedAsync] {DateTime.Now}: error");
                    _logger.Error(e);
                }

                _logger.Info($"[Sync Feed] Restart {DateTime.Now}");
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public async Task SyncFeedAsync(int sourceId)
        {
            while (true)
            {
                var source = _sourceRepository.Get(sourceId);
                if (source == null)
                {
                    return;
                }

                _logger.Info($"[{source.Unique}] Sync Feed: {source.Name}");

                var count = await _rssService.SyncFeedAsync(source);

                _logger.Info($"[{source.Unique}] Articles: {count}");

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        public async Task SyncFeedByTasksAsync()
        {
            while (true)
            {
                _logger.Info("[SyncFeedAsync]: start");

                try
                {
                    var sources = _sourceRepository.List();
                    var tasks = sources
                        .Select(source => _rssService.SyncFeedAsync(source))
                        .ToList();

                    while (tasks.Count > 0)
                    {
                        var completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                        tasks.Remove(completed);
                    }

                    _logger.Info("[SyncFeedAsync]: done");

                    _rssService.Clean();
                }
                catch (Exception e)
                {
                    _logger.Info("[SyncFeedAsync]: error");
                    _logger.Error(e);
                }

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
    }
}