using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class SourceItem : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Unique { get; set; }

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Name = Get<string>(r, "Name");
            Link = Get<string>(r, "Link");
            Unique = Get<string>(r, "Unique");
        }
    }
}