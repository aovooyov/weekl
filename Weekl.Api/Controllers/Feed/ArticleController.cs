using System;
using System.Linq;
using System.Web.Http;
using Weekl.Api.Models.Feed;
using Weekl.Core.Models;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Api.Controllers.Feed
{
    [RoutePrefix("feed/articles")]
    public class ArticleController : ApiController
    {
        private readonly IArticleRepository _articles;

        public ArticleController(IArticleRepository articles)
        {
            _articles = articles;
        }

        [Route("{date}")]
        [HttpGet]
        public IHttpActionResult Last(long date, int offset = 0, int take = 10)
        {            
            var articles = _articles
                .List(FilterXml.Empty, new DateTime(date), offset, take)
                .Select(a => new ArticleItemModel(a))
                .ToList();
            
            return Ok(articles);
        }

        [Route("~/feed/article/{articleId:int}")]
        [HttpGet]
        public IHttpActionResult GetArticle(int articleId)
        {
            var article = _articles.Get(articleId);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(new ArticleModel(article));
        }

        [Route("~/feed/article/{source}/{unique}")]
        [HttpGet]
        public IHttpActionResult GetArticle(string source, string unique)
        {
            var article = _articles.Get(source, unique);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(new ArticleModel(article));
        }
    }
}
