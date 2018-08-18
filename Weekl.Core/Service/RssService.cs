using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using NLog;
using ReadSharp;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Helper;
using Weekl.Core.Models;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Core.Service.Abstract;

namespace Weekl.Core.Service
{
    public class RssService : IRssService
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger _logger;

        public RssService(
            ISourceRepository sourceRepository,
            IChannelRepository channelRepository,
            IArticleRepository articleRepository)
        {
            _sourceRepository = sourceRepository;
            _channelRepository = channelRepository;
            _articleRepository = articleRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public IEnumerable<ArticleXml> GetFeed()
        {
            var sources = _sourceRepository.List();
            var articles = new List<ArticleXml>();

            foreach (var source in sources)
            {
                articles.AddRange(GetFeed(source));
            }

            return articles;
        }
        
        public IEnumerable<ArticleXml> GetFeed(int sourceId)
        {
            var source = _sourceRepository.Get(sourceId);
            if (source == null)
            {
                throw new NullReferenceException();
            }

            return GetFeed(source);
        }

        public IEnumerable<ArticleXml> GetFeed(Source source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            _logger.Info($"Source: {source.Name}");

            var channels = _channelRepository.ListBySourceId(source.Id);
            var articles = new List<ArticleXml>();

            _logger.Info($"Channels: {channels.Count()}");

            foreach (var channel in channels)
            {
                articles.AddRange(GetFeed(channel));
            }

            return articles;
        }

        public IEnumerable<ArticleXml> GetFeed(Channel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException();
            }

            _logger.Info($"Channel:\n{channel.Name}\n{channel.Link}");

            var articles = new List<ArticleXml>();
            Feed feed;

            try
            {
                var feedContent = RequestHelper.Get(channel.Link, channel.Encoding);
                feed = FeedReader.ReadFromString(feedContent);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return articles;
            }

            try
            {
                _sourceRepository.Update(
                    channel.SourceId,
                    feed.Description,
                    feed.ImageUrl
                );
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            
            var items = feed.Items
                .Where(i => i.PublishingDate?.Date >= channel.DateUpdated.Date)
                .ToList();

            if (items.Count == 0)
            {
                return articles;
            }

            var stopwatch = Stopwatch.StartNew();
            var times = new List<double>();

            foreach (var item in items)
            {
                Debug.WriteLine($"Categories: {string.Join("; ", item.Categories)}");
                Debug.WriteLine($"Content: {item.Content}");
                Debug.WriteLine($"Author: {item.Author}");
                Debug.WriteLine($"Id: {item.Id}");
                Debug.WriteLine($"Link: {item.Link}");

                var article = new ArticleXml
                {
                    ChannelId = channel.Id,
                    Title = item.Title,
                    SubTitle = item.Content,
                    Description = item.Description,
                    Link = item.Link,
                    Date = item.PublishingDate ?? DateTime.Now,
                    Category = string.Join("; ", item.Categories)
                };

                Debug.WriteLine(
                    $"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");

                try
                {
                    FillArticleContent(article);
                    Debug.WriteLine($"{article.Text}\n");

                    articles.Add(article);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

                var time = stopwatch.Elapsed.TotalMilliseconds;
                times.Add(time);
                stopwatch.Restart();

                _logger.Info(article.Title);
                _logger.Info(article.Link);
                _logger.Info($"Time: {time}");
            }

            _logger.Info($"Articles: {articles.Count}");
            _logger.Info($"Average: {times.Average()}");

            return articles;
        }

        public double SyncFeed(params int[] sourceIds)
        {
            var stopwatch = Stopwatch.StartNew();
            var sources = _sourceRepository.List(sourceIds);

            foreach (var source in sources)
            {
                _logger.Info($"Source: {source.Name}");

                var channels = _channelRepository.ListBySourceId(source.Id);

                _logger.Info($"Channels: {channels.Count()}");

                foreach (var channel in channels)
                {
                    var articles = GetFeed(channel);
                    if (!articles.Any())
                    {
                        continue;
                    }

                    try
                    {
                        _articleRepository.Import(articles);
                        _logger.Info("Articles imported success");
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e);
                    }
                }
            }

            _articleRepository.Clean();

            var time = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Stop();

            return time;
        }

        //public string GetArticleContent(string link, string queryCssSelector, string encoding)
        //{
        //    if (string.IsNullOrEmpty(queryCssSelector))
        //    {
        //        return string.Empty;
        //    }

        //    var web = new HtmlWeb()
        //    {
        //        AutoDetectEncoding = false,
        //        OverrideEncoding = Encoding.GetEncoding(encoding)
        //    };

        //    var document = web.Load(link);
        //    var contentNode = document.QuerySelector(queryCssSelector);
        //    var content = contentNode?.InnerHtml.Trim();

        //    return content;
        //}

        public void FillArticleContent(ArticleXml article)
        {
            if (string.IsNullOrEmpty(article?.Link))
            {
                return;
            }

            var contentTask = FillArticleContentAsync(article);
            contentTask.Wait();
        }

        public async Task FillArticleContentAsync(ArticleXml article)
        {
            if (string.IsNullOrEmpty(article?.Link))
            {
                return;
            }

            var reader = new Reader();

            try
            {
                var content = await reader.Read(new Uri(article.Link));

                article.Text = content.Content;

                if (content.Images.Any())
                {
                    foreach (var contentImage in content.Images)
                    {
                        var url = contentImage.Uri.ToString();
                        if (url.StartsWith("http") || url.StartsWith("data"))
                        {
                            article.Images.Add(url);
                        }
                    }
                }
            }
            catch (ReadException e)
            {
                _logger.Info($"Article link: {article.Link}");
                _logger.Error(e);
            }
        }
    }
}
