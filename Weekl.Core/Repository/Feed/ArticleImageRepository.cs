namespace Weekl.Core.Repository.Feed
{
    public class ArticleImageRepository : BaseRepository
    {
        public ArticleImageRepository(string connectionString) : base(connectionString)
        {
        }

        public void Add(int articleId, string link)
        {
            var parameters = new[]
            {
                GetParameter("@articleId", articleId),
                GetParameter("@link", link)
            };

            ExecuteNonQuery("FEED.ArticleImageAdd", parameters);
        }
    }
}