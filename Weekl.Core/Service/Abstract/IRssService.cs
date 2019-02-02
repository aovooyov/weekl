using System.Collections.Generic;
using System.Threading.Tasks;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Models;

namespace Weekl.Core.Service.Abstract
{
    public interface IRssService
    {
        //Task<ICollection<ArticleXml>> GetFeed();
        //Task<ICollection<ArticleXml>> GetFeed(SourceItem source);
        //Task<ICollection<ArticleXml>> GetFeed(ChannelItem channel);

        Task<int> SyncFeedAsync();
        Task<int> SyncFeedAsync(SourceItem source);
        Task<int> SyncFeedAsync(ChannelItem channel);

        Task FillArticleContentAsync(ArticleXml article);
        void Clean();
    }
}