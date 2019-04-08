using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Weekl.Core.Models;
using Weekl.Core.Service.Abstract;

namespace Weekl.Core.Service
{
    public sealed class LostcutService : ILostcutService
    {
        private const string HostUrl = "http://api.lostcut.net";
        private const string MoviesUrl = "hdgo/videos";

        public async Task<ICollection<Movie>> GetMoviesAsync(int start = 0, int limit = 100)
        {
            using (var client = new HttpClient())
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["start"] = start.ToString();
                parameters["limit"] = limit.ToString();

                var uriBuilder = new UriBuilder(HostUrl);
                uriBuilder.Path = MoviesUrl;
                uriBuilder.Query = parameters.ToString();

                var response = await client.GetStringAsync(uriBuilder.Uri);
                return JsonConvert.DeserializeObject<ICollection<Movie>>(response);
            }
        }

        public async Task<ICollection<Movie>> GetMoviesAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);

                if (int.TryParse(query, out int kpid))
                {
                    parameters["kpid"] = query;
                }
                else
                {
                    parameters["q"] = query;
                }

                var uriBuilder = new UriBuilder(HostUrl);
                uriBuilder.Path = MoviesUrl;
                uriBuilder.Query = parameters.ToString();

                var response = await client.GetStringAsync(uriBuilder.Uri);
                return JsonConvert.DeserializeObject<ICollection<Movie>>(response);
            }
        }
    }

}
