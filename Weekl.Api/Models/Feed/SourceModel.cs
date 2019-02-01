using Weekl.Core.Entities.Feed;

namespace Weekl.Api.Models.Feed
{
    public class SourceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Unique { get; set; }
        public string ImageUrl { get; set; }

        public SourceModel()
        {
            
        }

        public SourceModel(int sourceId, string name, string link, string imageUrl, string unique)
        {
            Id = sourceId;
            Name = name;
            Link = link;
            ImageUrl = imageUrl;
            Unique = unique;
        }

        public SourceModel(SourceItem source)
        {
            Id = source.Id;
            Name = source.Name;
            Link = source.Link;
            Unique = source.Unique;
        }
    }
}