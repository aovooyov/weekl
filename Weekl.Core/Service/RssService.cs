using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

        public async Task<ICollection<ArticleXml>> GetFeed()
        {
            var sources = _sourceRepository.List();
            var articles = new List<ArticleXml>();

            foreach (var source in sources)
            {
                articles.AddRange(await GetFeed(source));
            }

            return articles;
        }
        
        public async Task<ICollection<ArticleXml>> GetFeed(SourceItem source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            var channels = _channelRepository.ListBySourceId(source.Id);
            var articles = new List<ArticleXml>();

            _logger.Info($"[{source.Unique}] Source: {source.Name}, Channels: {channels.Count()}");

            foreach (var channel in channels)
            {
                articles.AddRange(await GetFeed(channel));
            }

            return articles;
        }

        public async Task<ICollection<ArticleXml>> GetFeed(ChannelItem channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException();
            }

            _logger.Info($"[{channel.SourceUnique}] Channel: {channel.Name} {channel.Link}");

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
                //Debug.WriteLine($"Categories: {string.Join("; ", item.Categories)}");
                //Debug.WriteLine($"Content: {item.Content}");
                //Debug.WriteLine($"Author: {item.Author}");
                //Debug.WriteLine($"Id: {item.Id}");
                //Debug.WriteLine($"Link: {item.Link}");


                DateTime? date = null;
                if (!string.IsNullOrEmpty(item.PublishingDateString))
                {
                    var dateString = Regex.Replace(item.PublishingDateString, "\\+0(\\d)00", string.Empty);
                    date = DateTime.Parse(dateString);
                }

                var article = new ArticleXml
                {
                    ChannelId = channel.Id,
                    Title = item.Title,
                    SubTitle = item.Content,
                    Description = item.Description,
                    Link = item.Link,
                    Date = date ?? item.PublishingDate ?? DateTime.Now,
                    Category = string.Join("; ", item.Categories)
                };

                //Debug.WriteLine(
                //    $"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");

                await FillArticleContentAsync(article);

                articles.Add(article);

                var time = stopwatch.Elapsed.TotalMilliseconds;
                times.Add(time);
                stopwatch.Restart();
                
                //Debug.WriteLine(article.Title);
                //Debug.WriteLine(article.Link);
                //Debug.WriteLine($"Time: {time}");
            }

            //Debug.WriteLine($"Articles: {articles.Count}");
            //Debug.WriteLine($"Average: {times.Average()}");

            return articles;
        }

        public async Task<int> SyncFeed()
        {
            var stopwatch = Stopwatch.StartNew();
            var sources = _sourceRepository.List();
            var articleCount = 0;

            foreach (var source in sources)
            {
                var channels = _channelRepository.ListBySourceId(source.Id);

                _logger.Info($"[{source.Unique}] Source: {source.Name}, Channels: {channels.Count()}");

                foreach (var channel in channels)
                {
                    articleCount += await SyncFeed(channel);
                }
            }

            var time = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Stop();
            _logger.Info($"Total time: {time}");

            return articleCount;
        }

        public async Task<int> SyncFeed(SourceItem source)
        {
            if (source == null)
            {
                return 0;
            }

            var channels = _channelRepository.ListBySourceId(source.Id);
            var articleCount = 0;

            _logger.Info($"[{source.Unique}] {source.Name}, Channels: {channels.Count}");

            foreach (var channel in channels)
            {
                articleCount += await SyncFeed(channel);
            }

            return articleCount;
        }

        public async Task<int> SyncFeed(ChannelItem channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException();
            }

            _logger.Info($"[{channel.SourceUnique}] Channel: {channel.Name} {channel.Link}");

            Feed feed;

            try
            {
                var feedContent = RequestHelper.Get(channel.Link, channel.Encoding);
                feed = FeedReader.ReadFromString(feedContent);
            }
            catch (Exception e)
            {
                _logger.Error($"[{channel.SourceUnique}] Error: {e.Message}");
                _logger.Error(e.StackTrace);
                return 0;
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
                return 0;
            }

            var articlesCount = 0;
            var articles = new List<ArticleXml>();

            foreach (var item in items)
            {
                //Debug.WriteLine($"Categories: {string.Join("; ", item.Categories)}");
                //Debug.WriteLine($"Content: {item.Content}");
                //Debug.WriteLine($"Author: {item.Author}");
                //Debug.WriteLine($"Id: {item.Id}");
                //Debug.WriteLine($"Link: {item.Link}");

                DateTime? date = null;
                if (!string.IsNullOrEmpty(item.PublishingDateString))
                {
                    var dateString = Regex.Replace(item.PublishingDateString, "\\+0(\\d)00", string.Empty);
                    date = DateTime.Parse(dateString);

                    _logger.Trace($"[{channel.SourceUnique}] PublishingDateString: {item.PublishingDateString}, PublishingDate: {item.PublishingDate}, Date: {date}");
                }

                var article = new ArticleXml
                {
                    ChannelId = channel.Id,
                    Title = item.Title,
                    SubTitle = item.Content,
                    Description = item.Description,
                    Link = item.Link,
                    Date = date ?? item.PublishingDate ?? DateTime.Now,
                    Category = string.Join("; ", item.Categories)
                };

                //Debug.WriteLine(
                //    $"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");

                await FillArticleContentAsync(article);

                articles.Add(article);

                if (articles.Count < 10)
                {
                    continue;
                }

                try
                {
                    _articleRepository.Import(articles);

                    _logger.Info($"[{channel.SourceUnique}] Articles: {articles.Count}");
                    articlesCount += articles.Count;
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }

                articles.Clear();
            }

            if (articles.Count == 0)
            {
                return articlesCount;
            }
            
            try
            {
                _articleRepository.Import(articles);

                _logger.Info($"[{channel.SourceUnique}] Articles: {articles.Count}");
                articlesCount += articles.Count;
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            articles.Clear();
            
            return articlesCount;
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
                _logger.Error($"Article: {article.Link}", e);
            }
        }

        public void Clean()
        {
            _articleRepository.Clean();
        }
    }
}

