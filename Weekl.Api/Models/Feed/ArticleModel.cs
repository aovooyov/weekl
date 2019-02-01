using System;
using Weekl.Core.Entities.Feed;

namespace Weekl.Api.Models.Feed
{
    public class ArticleModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
        public string Unqiue { get; set; }

        public int ChannelId { get; set; }
        public string ChannelName { get; set; }

        public SourceModel Source { get; set; }

        public ArticleModel()
        {
            Source = new SourceModel();
        }

        public ArticleModel(Article article)
        {
            Id = article.Id;
            Link = article.Link;
            Title = article.Title;
            SubTitle = article.SubTitle;
            Description = article.Description;
            Text = article.Text;
            Date = article.Date;
            ImageUrl = article.ImageUrl;
            Unqiue = article.Unique;

            ChannelId = article.ChannelId;
            ChannelName = article.ChannelName;

            Source = new SourceModel(
                article.SourceId,
                article.SourceName,
                article.SourceLink,
                article.SourceImageUrl,
                article.SourceUnique);
        }
    }
}