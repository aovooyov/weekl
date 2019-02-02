using System.Threading.Tasks;

namespace Weekl.Service.Worker
{
    public interface IWorker
    {
        Task SyncFeedAsync();
        Task SyncFeedAsync(int sourceId);
        Task SyncFeedByTasksAsync();
    }
}