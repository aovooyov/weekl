using System.Collections.Generic;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Types;

namespace Weekl.Core.Repository.Feed.Abstract
{
    public interface IChannelRepository
    {
        Channel Add(int sourceId, string name, string link, string selector, string encoding, ChannelType type);
        Channel Update(int channelId, string name, string link, string selector, string encoding, ChannelType type);
        Channel Get(int channelId);
        ICollection<Channel> List();
        ICollection<ChannelItem> ListBySourceId(int sourceId);
    }
}