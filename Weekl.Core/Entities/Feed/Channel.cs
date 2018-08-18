using System;
using System.Data.Common;
using Weekl.Core.Types;

namespace Weekl.Core.Entities.Feed
{
    public class Channel : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Selector { get; set; }
        public string Encoding { get; set; }
        public ChannelType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            SourceId = Get<int>(r, "SourceId");
            Name = Get<string>(r, "Name");
            Link = Get<string>(r, "Link");
            Selector = Get<string>(r, "Selector");
            Encoding = Get<string>(r, "Encoding");
            Type = (ChannelType)Get<int>(r, "Type");
            DateCreated = Get<DateTime>(r, "DateCreated");
            DateUpdated = Get<DateTime>(r, "DateUpdated");
        }
    }
}