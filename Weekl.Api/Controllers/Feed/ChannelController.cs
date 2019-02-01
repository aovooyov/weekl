using System.Linq;
using System.Web.Http;
using Weekl.Api.Models.Feed;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Api.Controllers.Feed
{
    [RoutePrefix("feed/channels")]
    public class ChannelController : ApiController
    {
        private readonly IChannelRepository _channels;

        public ChannelController(IChannelRepository channels)
        {
            _channels = channels;
        }

        [Route("{sourceId}")]
        [HttpGet]
        public IHttpActionResult List(int sourceId)
        {
            var channels = _channels.ListBySourceId(sourceId);
            return Ok(channels.Select(c => new ChannelItemModel(c)).ToList());
        }
    }
}