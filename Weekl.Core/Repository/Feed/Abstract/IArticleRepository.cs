using System;
using System.Collections.Generic;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Models;

namespace Weekl.Core.Repository.Feed.Abstract
{
    public interface IArticleRepository
    {
        Article Add(int channelId, string title, string subtitle, string description, string text, string link, DateTime date);
        Article Update(int articleId, string title, string subtitle, string description, string text, string link, DateTime date);
        Article Get(int articleId);
        Article Get(string source, string unique);
        ICollection<ArticleItem> List(FilterXml filter, DateTime date, int offset, int take);
        void Import(ICollection<ArticleXml> articles);
        void Clean();
    }
}