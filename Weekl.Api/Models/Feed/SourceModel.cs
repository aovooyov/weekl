using Weekl.Core.Entities.Feed;

namespace Weekl.Api.Models.Feed
{
    public class SourceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public SourceModel()
        {
            
        }

        public SourceModel(Source source)
        {
            Id = source.Id;
            Name = source.Name;
            Link = source.Link;
            Description = source.Description;
        }
    }
}