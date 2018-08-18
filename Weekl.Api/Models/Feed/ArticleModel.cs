using System;
using Weekl.Core.Entities.Feed;

namespace Weekl.Api.Models.Feed
{
    public class ArticleModel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }

        public ArticleModel()
        {
            
        }

        public ArticleModel(Article article)
        {
            Id = article.Id;
            ChannelId = article.ChannelId;
            Title = article.Title;
            SubTitle = article.SubTitle;
            Description = article.Description;
            Text = article.Text;
            Link = article.Link;
            Date = article.Date;
        }
    }
}