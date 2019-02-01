using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class Article : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
        public string Unique { get; set; }

        public int ChannelId { get; set; }
        public string ChannelName { get; set; }

        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceLink { get; set; }
        public string SourceImageUrl { get; set; }
        public string SourceUnique { get; set; }

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Link = Get<string>(r, "Link");
            Title = Get<string>(r, "Title");
            SubTitle = Get<string>(r, "SubTitle");
            Description = Get<string>(r, "Description");
            Text = Get<string>(r, "Text");
            ImageUrl = Get<string>(r, "ImageUrl");
            Date = Get<DateTime>(r, "Date");
            Unique = Get<string>(r, "Unique");

            ChannelId = Get<int>(r, "ChannelId");
            ChannelName = Get<string>(r, "ChannelName");

            SourceId = Get<int>(r, "SourceId");
            SourceName = Get<string>(r, "SourceName");
            SourceLink = Get<string>(r, "SourceLink");
            SourceImageUrl = Get<string>(r, "SourceImageUrl");
            SourceUnique = Get<string>(r, "SourceUnique");
        }
    }
}
