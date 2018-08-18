using System.Collections.Generic;
using System.Threading.Tasks;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Models;

namespace Weekl.Core.Service.Abstract
{
    public interface IRssService
    {
        IEnumerable<ArticleXml> GetFeed();
        IEnumerable<ArticleXml> GetFeed(int sourceId);
        IEnumerable<ArticleXml> GetFeed(Source source);
        IEnumerable<ArticleXml> GetFeed(Channel channel);

        double SyncFeed(params int[] sourceIds);
        void FillArticleContent(ArticleXml article);
        Task FillArticleContentAsync(ArticleXml article);
    }
}