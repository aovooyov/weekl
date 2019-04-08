using System;
using System.Threading.Tasks;
using System.Web.Http;
using Weekl.Api.Models.Movie;
using Weekl.Core.Service.Abstract;

namespace Weekl.Api.Controllers.Movie
{
    [RoutePrefix("movie")]
    public class MovieController : ApiController
    {
        private readonly ILostcutService _lostcutService;

        public MovieController(ILostcutService lostcutService)
        {
            _lostcutService = lostcutService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> MovieInfo(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Ok(new Core.Models.Movie[] { });
            }

            try
            {
                var movies = await _lostcutService.GetMoviesAsync(query);
                return Ok(movies);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}