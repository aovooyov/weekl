using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weekl.Core.Models;

namespace Weekl.Core.Service.Abstract
{
    public interface ILostcutService
    {
        Task<ICollection<Movie>> GetMoviesAsync(int start = 0, int limit = 100);
        Task<ICollection<Movie>> GetMoviesAsync(string query);
    }
}
