using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weekl.Core.Helper;
using Weekl.Core.Models;
using Weekl.Core.Repository.Feed;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Core.Service;
using Weekl.Core.Service.Abstract;

namespace Weekl.ConsoleApp
{
    public class WeeklConsole
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private readonly ISourceRepository _sourceRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IRssService _rssService;

        public WeeklConsole()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Weekl"].ConnectionString;

            _sourceRepository = new SourceRepository(connectionString);
            _channelRepository = new ChannelRepository(connectionString);
            _articleRepository = new ArticleRepository(connectionString);

            _rssService = new RssService(_sourceRepository, _channelRepository, _articleRepository);
        }

        //public async Task GetFeed()
        //{
        //    var times = new List<double>();

        //    var sources = _sourceRepository.List();

        //    foreach (var source in sources)
        //    {
        //        Console.WriteLine($"{source.Name}\n");

        //        var time = _stopwatch.Elapsed.TotalMilliseconds;
        //        times.Add(time);
        //        _stopwatch.Restart();

        //        var articles = await _rssService.GetFeed(source);

        //        foreach (var article in articles)
        //        {
        //            Console.WriteLine($"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");
        //            Console.WriteLine($"{article.Text}\n");
        //        }

        //        Console.WriteLine($"\n{time}\n");
        //    }

        //    Console.WriteLine($"Average time {times.Average()}");
        //}

        public async Task SyncFeedAsync()
        {
            while (true)
            {
                Console.WriteLine("[SyncFeedAsync]: start");

                var total = 0;
                try
                {
                    total = await _rssService.SyncFeedAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SyncFeedAsync]: error");
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    Console.WriteLine($"[SyncFeedAsync]: total articles {total}");
                    Console.WriteLine("[SyncFeedAsync]: clean...");

                    _rssService.Clean();

                    Console.WriteLine("[SyncFeedAsync]: done");

                    await Task.Delay(TimeSpan.FromMinutes(5));
                }
            }
        }

        public async Task SyncFeedByTasksAsync()
        {
            while (true)
            {
                Console.WriteLine("[SyncFeedAsync]: start");

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

                        Console.WriteLine($"total time: {completed.Result}");
                    }

                    Console.WriteLine("[SyncFeedAsync]: done");

                    _rssService.Clean();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[SyncFeedAsync]: error");
                    Console.WriteLine(e.ToString());
                }

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        public void Import()
        {
            const string filename = "Weekl.csv";
            var sources = new List<SourceXml>();

            foreach (var line in File.ReadLines(filename))
            {
                var item = line
                    .Replace("\"", "&quot;")
                    .Replace("'", "&quot;")
                    .Replace("&", "&amp;");

                var source = item.Split(',');

                sources.Add(new SourceXml
                {
                    Name = source[0],
                    Source = source[1],
                    Channel = source[2],
                    Selector = source[3],
                    Encoding = string.IsNullOrEmpty(source[4]) ? "utf-8" : source[4]
                });
            }

            _sourceRepository.Import(sources);
        }
    }
}