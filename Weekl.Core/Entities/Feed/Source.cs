using System;
using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class Source : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated {get; set;}

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Name = Get<string>(r, "Name");
            Link = Get<string>(r, "Link");
            Description = Get<string>(r, "Description");
            DateCreated = Get<DateTime>(r, "DateCreated");
        }
    }
}
