using System.Linq;
using System.Web.Http;
using Weekl.Api.Models.Feed;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Api.Controllers.Feed
{
    [RoutePrefix("feed/sources")]
    public class SourceController : ApiController
    {
        private readonly ISourceRepository _sources;

        public SourceController(ISourceRepository sources)
        {
            _sources = sources;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List()
        {
            var sources = _sources.List();
            return Ok(sources.Select(s => new SourceModel(s)).ToList());
        }
    }
}