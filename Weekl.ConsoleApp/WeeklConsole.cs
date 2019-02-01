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

        public async Task ReadFeed()
        {
            var times = new List<double>();

            var sources = _sourceRepository.List();

            foreach (var source in sources)
            {
                Console.WriteLine($"{source.Name}\n");

                var time = _stopwatch.Elapsed.TotalMilliseconds;
                times.Add(time);
                _stopwatch.Restart();

                var articles = await _rssService.GetFeed(source);

                foreach (var article in articles)
                {
                    Console.WriteLine($"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");
                    Console.WriteLine($"{article.Text}\n");
                }

                Console.WriteLine($"\n{time}\n");
            }

            Console.WriteLine($"Average time {times.Average()}");
        }

        public void ReedFeedInfo()
        {
            _rssService.GetFeed();
        }

        public void SyncFeed()
        {
            var time = _rssService.SyncFeed();

            Console.WriteLine($"total time: {time}");
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

        public void ImportManually()
        {
            var sources = new List<SourceXml>
            {
                new SourceXml
                {
                    Name = "Газета.Ru - Первая полоса",
                    Source = "https://www.gazeta.ru",
                    Channel = "https://www.gazeta.ru/export/rss/first.xml",
                    Encoding = "ISO-8859-1"
                },
                new SourceXml
                {
                    Name = "Газета.Ru - Новости дня",
                    Source = "https://www.gazeta.ru",
                    Channel = "https://www.gazeta.ru/export/rss/lenta.xml",
                    Encoding = "ISO-8859-1"
                }
            };

            _sourceRepository.Import(sources);
        }

        public void DefaultSourceMain()
        {


            //var connectionString = ConfigurationManager.ConnectionStrings["Weekl"].ConnectionString;
            //var sourceRepository = new SourceRepository(connectionString);

            //var source = sourceRepository.Add(
            //    "Community",
            //    "https://thecommunity.ru",
            //    "https://thecommunity.ru/rss.xml",
            //    "body #full-story",
            //    @"Community - новостной портал об IT-технологиях\nРегулярно на сайте публикуются самые свежие и актуальные новости из мира IT-технологий, интересные статьи и материалы с различных конференций.",
            //    "utf-8");

            //Console.WriteLine($"{source.Id} {source.Name} {source.Link} {source.Rss} {source.Selector}");

            //source = sourceRepository.Add(
            //    "Лента.Ру (Новости)",
            //    "http://lenta.ru",
            //    "http://lenta.ru/rss/news",
            //    "body .b-topic-layout .b-topic-layout__content .b-topic-layout__left .b-topic__content .b-text",
            //    @"Lenta.Ru (Лента.Ру)",
            //    "utf-8");

            //Console.WriteLine($"{source.Id} {source.Name} {source.Link} {source.Rss} {source.Selector}");

            //source = sourceRepository.Add(
            //    "Лента.Ру (Самые свежие и самые важные новости)",
            //    "http://lenta.ru",
            //    "http://lenta.ru/rss/top7",
            //    "body .b-topic-layout .b-topic-layout__content .b-topic-layout__left .b-topic__content .b-text",
            //    @"Lenta.Ru (Лента.Ру)",
            //    "utf-8");

            //Console.WriteLine($"{source.Id} {source.Name} {source.Link} {source.Rss} {source.Selector}");

            //source = sourceRepository.Add(
            //    "TJ: все новости и статьи",
            //    "https://tjournal.ru",
            //    "https://tjournal.ru/rss/all",
            //    "body .entry_content--full .b-article",
            //    @"TJ",
            //    "utf-8");

            //Console.WriteLine($"{source.Id} {source.Name} {source.Link} {source.Rss} {source.Selector}");

            //source = sourceRepository.Add(
            //    "КиноПоиск: Новости",
            //    "https://www.kinopoisk.ru",
            //    "https://www.kinopoisk.ru/news.rss",
            //    "body .article .article__content",
            //    @"Свежая и интересная информация из мира кино: новости, репортажи, рецензии, трейлеры...",
            //    "windows-1251");

            //Console.WriteLine($"{source.Id} {source.Name} {source.Link} {source.Rss} {source.Selector}");
        }

    }
}