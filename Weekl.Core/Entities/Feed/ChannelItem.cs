using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class ChannelItem : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Encoding { get; set; }
        public string Selector { get; set; }
        public DateTime DateUpdated { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceLink { get; set; }
        public string SourceImageUrl { get; set; }
        public string SourceUnique { get; set; }
        
        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Name = Get<string>(r, "Name");
            Link = Get<string>(r, "Link");
            Encoding = Get<string>(r, "Encoding");
            Selector = Get<string>(r, "Selector");
            DateUpdated = Get<DateTime>(r, "DateUpdated");
            SourceId = Get<int>(r, "SourceId");
            SourceName = Get<string>(r, "SourceName");
            SourceLink = Get<string>(r, "SourceLink");
            SourceImageUrl = Get<string>(r, "SourceImageUrl");
            SourceUnique = Get<string>(r, "SourceUnique");
        }
    }
}