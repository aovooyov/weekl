using System;
using System.Collections.Generic;
using System.Data;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Helper;
using Weekl.Core.Models;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Core.Repository.Feed
{
    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public ArticleRepository(string connectionString) : base(connectionString)
        {
        }

        public Article Add(int channelId, string title, string subtitle, string description, string text, string link, DateTime date)
        {
            var parameters = new[]
            {
                GetParameter("@channelId", channelId),
                GetParameter("@title", title),
                GetParameter("@subtitle", subtitle),
                GetParameter("@description", description),
                GetParameter("@text", text),
                GetParameter("@link", link),
                GetParameter("@date", date)
            };

            return Get<Article>("FEED.ArticleAdd", CommandType.StoredProcedure, parameters);
        }

        public Article Update(int articleId, string title, string subtitle, string description, string text, string link, DateTime date)
        {
            var parameters = new[]
            {
                GetParameter("@articleId", articleId),
                GetParameter("@title", title),
                GetParameter("@subtitle", subtitle),
                GetParameter("@description", description),
                GetParameter("@text", text),
                GetParameter("@link", link),
                GetParameter("@date", date)
            };

            return Get<Article>("FEED.ArticleUpdate", CommandType.StoredProcedure, parameters);
        }

        public Article Get(int articleId)
        {
            return Get<Article>("FEED.ArticleGet", CommandType.StoredProcedure, GetParameter("@articleId", articleId), GetParameter("@source", (string)null), GetParameter("@unique", (string)null));
        }

        public Article Get(string source, string unique)
        {
            return Get<Article>("FEED.ArticleGet", CommandType.StoredProcedure, GetParameter("@articleId", (int?)null), GetParameter("@source", source), GetParameter("@unique", unique));
        }

        public IEnumerable<ArticleItem> List(string filter, DateTime date, int offset, int take)
        {
            var parameters = new[]
            {
                GetParameter("@filter", filter),
                GetParameter("@date", date),
                GetParameter("@offset", offset),
                GetParameter("@take", take)
            };

            return GetList<ArticleItem>("FEED.ArticleList", CommandType.StoredProcedure, parameters);
        }

        public void Import(IEnumerable<ArticleXml> articles)
        {
            var xml = XmlHelper.ArticlesToXml(articles);
            ExecuteNonQuery("FEED.ArticleImportXml", GetParameter("@articles", xml));
        }

        public void Clean()
        {
            ExecuteNonQuery("FEED.ArticleClean");
        }
    }
}
