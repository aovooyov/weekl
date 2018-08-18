using System.Collections.Generic;
using System.Data;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Core.Types;

namespace Weekl.Core.Repository.Feed
{
    public class ChannelRepository : BaseRepository, IChannelRepository
    {
        public ChannelRepository(string connectionString) : base(connectionString)
        {
        }

        public Channel Add(int sourceId, string name, string link, string selector, string encoding, ChannelType type)
        {
            var parameters = new[]
{
                GetParameter("@sourceId", sourceId),
                GetParameter("@name", name),
                GetParameter("@link", link),
                GetParameter("@selector", selector),
                GetParameter("@encoding", encoding),
                GetParameter("@type", type)
            };

            return Get<Channel>("FEED.ChannelAdd", CommandType.StoredProcedure, parameters);
        }

        public Channel Update(int channelId, string name, string link, string selector, string encoding, ChannelType type)
        {
            var parameters = new[]
            {
                GetParameter("@channelId", channelId),
                GetParameter("@name", name),
                GetParameter("@link", link),
                GetParameter("@selector", selector),
                GetParameter("@encoding", encoding),
                GetParameter("@type", type)
            };

            return Get<Channel>("FEED.ChannelUpdate", CommandType.StoredProcedure, parameters);
        }

        public Channel Get(int channelId)
        {
            return Get<Channel>("FEED.ChannelGet", CommandType.StoredProcedure, GetParameter("@channelId", channelId));
        }

        public IEnumerable<Channel> List()
        {
            return GetList<Channel>("FEED.ChannelList", CommandType.StoredProcedure);
        }

        public IEnumerable<Channel> ListBySourceId(int sourceId)
        {
            return GetList<Channel>("FEED.ChannelListBySourceId", CommandType.StoredProcedure, GetParameter("@sourceId", sourceId));
        }
    }
}