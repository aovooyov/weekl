using System.Web.Http;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Api.Controllers.Admin
{
    [RoutePrefix("admin/sources")]
    public class AdminSourceController : ApiController
    {
        private readonly IChannelRepository _channels;
        private readonly ISourceRepository _sources;

        public AdminSourceController(
            IChannelRepository channels,
            ISourceRepository sources)
        {
            _channels = channels;
            _sources = sources;
        }


    }
}