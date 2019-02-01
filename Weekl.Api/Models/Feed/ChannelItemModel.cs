using System;
using Weekl.Core.Entities.Feed;

namespace Weekl.Api.Models.Feed
{
    public class ChannelItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Encoding { get; set; }
        public string Selector { get; set; }
        public DateTime DateUpdated { get; set; }
        public int SourceId { get; set; }

        public ChannelItemModel(ChannelItem channel)
        {
            Id = channel.Id;
            Name = channel.Name;
            Link = channel.Link;
            Encoding = channel.Encoding;
            Selector = channel.Selector;
            DateUpdated = channel.DateUpdated;
            SourceId = channel.SourceId;
        }
    }
}