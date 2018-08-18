using System;
using System.Collections.Generic;

namespace Weekl.Api.Models.Feed
{
    public class ArticlesModel
    {
        public DateTime Date { get; set; }
        public ICollection<ArticleModel> Articles { get; set; }

        public ArticlesModel()
        {
            Articles = new HashSet<ArticleModel>();
        }
    }
}