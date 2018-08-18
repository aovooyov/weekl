using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class ArticleItem : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
        public string Unique { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceLink { get; set; }
        public string SourceImageUrl { get; set; }
        public string SourceUnique { get; set; }

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Title = Get<string>(r, "Title");
            Description = Get<string>(r, "Description");
            Link = Get<string>(r, "Link");
            Date = Get<DateTime>(r, "Date");
            ImageUrl = Get<string>(r, "ImageUrl");
            Unique = Get<string>(r, "Unique");
            SourceId = Get<int>(r, "SourceId");
            SourceName = Get<string>(r, "SourceName");
            SourceLink = Get<string>(r, "SourceLink");
            SourceImageUrl = Get<string>(r, "SourceImageUrl");
            SourceUnique = Get<string>(r, "SourceUnique");
        }
    }
}