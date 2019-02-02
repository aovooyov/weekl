using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using HtmlAgilityPack;
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

        private ICollection<string> _ignoreList;
        private ICollection<string> IgnoreList
        {
            get
            {
                return _ignoreList ?? (_ignoreList = _articleRepository
                           .IgnoreList()
                           .Select(i => i.Cause)
                           .ToList());
            }
        }

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

        #region GetFeed
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

                try
                {
                    await FillArticleContentAsync(article);
                    articles.Add(article);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }

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
        #endregion

        public async Task<int> SyncFeedAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var sources = _sourceRepository.List();
            var articleCount = 0;

            foreach (var source in sources)
            {
                var channels = _channelRepository.ListBySourceId(source.Id);

                _logger.Info($"[{source.Unique}]: Source {source.Name}, Channels: {channels.Count()}");

                foreach (var channel in channels)
                {
                    var count = await SyncFeedAsync(channel);
                    articleCount += count;
                }
            }

            var time = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Stop();
            _logger.Info($"[{DateTime.Now}]: Total time {time}");

            return articleCount;
        }

        public async Task<int> SyncFeedAsync(SourceItem source)
        {
            if (source == null)
            {
                return 0;
            }

            var channels = _channelRepository.ListBySourceId(source.Id);
            var articleCount = 0;

            _logger.Info($"[{source.Unique}]: {source.Name} Channels: {channels.Count}");

            foreach (var channel in channels)
            {
                articleCount += await SyncFeedAsync(channel);
            }

            return articleCount;
        }

        public async Task<int> SyncFeedAsync(ChannelItem channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException();
            }

            _logger.Info($"[{channel.SourceUnique}]: Channel {channel.Name} {channel.Link}");

            Feed feed;

            try
            {
                var feedContent = RequestHelper.Get(channel.Link, channel.Encoding);
                feed = FeedReader.ReadFromString(feedContent);
            }
            catch (Exception e)
            {
                _logger.Error($"[{channel.SourceUnique}]: {e.Message}");
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

                    //_logger.Trace($"[{channel.SourceUnique}] PublishingDateString: {item.PublishingDateString}, PublishingDate: {item.PublishingDate}, Date: {date}");
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

                Debug.WriteLine(
                    $"{article.Date}\n{article.Title}\n{article.SubTitle}\n{article.Description}\n{article.Link}\n{article.Category}\n\n");

                try
                {
                    await FillArticleContentAsync(article);
                    articles.Add(article);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }

                if (articles.Count < 10)
                {
                    continue;
                }

                try
                {
                    _articleRepository.Import(articles);

                    _logger.Info($"[{channel.SourceUnique}]: Articles {articles.Count}");
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

                _logger.Info($"[{channel.SourceUnique}]: Articles {articles.Count}");
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
                var contentArticle = await reader.Read(new Uri(article.Link));
                
                var document = new HtmlDocument();
                document.LoadHtml(contentArticle.Content);

                var imgNodes = document.QuerySelectorAll("img");
                var ignoreList = IgnoreList.ToArray();
                var ignoreImgNodes = imgNodes
                    .Where(img => ignoreList.Contains(img.GetAttributeValue("src", string.Empty)))
                    .ToList();

                if (ignoreImgNodes.Count > 0)
                {
                    foreach (var imgNode in ignoreImgNodes)
                    {
                        _logger.Info($"[{DateTime.Now}]: Article Content Ignore {imgNode.GetAttributeValue("src", string.Empty)}");
                        imgNode.Remove();
                    }
                }
                
                article.Text = document.DocumentNode.InnerHtml?.Trim();

                if (contentArticle.Images.Any())
                {
                    foreach (var contentImage in contentArticle.Images)
                    {
                        if (contentImage?.Uri == null)
                        {
                            continue;
                        }

                        //_logger.Info($"Article image: {contentImage.Uri}");

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
            _logger.Info($"[{DateTime.Now}]: Clean Articles");

            _articleRepository.Clean();
        }
    }
}

