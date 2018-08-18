namespace Weekl.Core.Repository.Feed.Abstract
{
    public interface IArticleImageRepository
    {
        void Add(int articleId, string link);
    }
}
