using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class Article : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        
        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            ChannelId = Get<int>(r, "ChannelId");
            Title = Get<string>(r, "Title");
            SubTitle = Get<string>(r, "SubTitle");
            Description = Get<string>(r, "Description");
            Text = Get<string>(r, "Text");
            Link = Get<string>(r, "Link");
            Date = Get<DateTime>(r, "Date");
            DateCreated = Get<DateTime>(r, "DateCreated");
        }
    }
}
