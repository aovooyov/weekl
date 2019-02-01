using System.Threading.Tasks;

namespace Weekl.Service.Worker
{
    public interface IWorker
    {
        Task SyncFeed();
        Task SyncFeed(int sourceId);
        Task SyncFeedAsync();
    }
}