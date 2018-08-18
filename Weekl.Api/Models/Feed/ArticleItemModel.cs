using System;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Extensions;

namespace Weekl.Api.Models.Feed
{
    public class ArticleItemModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
        public string Unique { get; set; }
        public SourceItemModel Source { get; set; }

        public ArticleItemModel(ArticleItem article)
        {
            Id = article.Id;
            Title = article.Title;
            Link = article.Link;
            Date = article.Date;
            Unique = article.Unique;

            var imgCount = article.Description?.ContainsCount("<img") ?? 0;
            switch (imgCount)
            {
                case 0:
                    Description = article.Description.StripScriptTag();
                    ImageUrl = article.ImageUrl;
                    break;
                case 1:
                    Description = article.Description.StripImgTag().StripScriptTag();
                    ImageUrl = article.ImageUrl;
                    break;
                default:
                    Description = article.Description.StripScriptTag();
                    break;
            }

            Source = new SourceItemModel(article.SourceId, article.SourceName, article.SourceLink, article.SourceImageUrl, article.SourceUnique);
        }
    }
}