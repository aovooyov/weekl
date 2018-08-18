namespace Weekl.Api.Models.Feed
{
    public class SourceItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Unique { get; set; }

        public SourceItemModel(int sourceId, string name, string link, string imageUrl, string unique)
        {
            Id = sourceId;
            Name = name;
            Link = link;
            ImageUrl = imageUrl;
            Unique = unique;
        }
    }
}