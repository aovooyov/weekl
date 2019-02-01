using System.Collections.Generic;
using System.Data;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Helper;
using Weekl.Core.Models;
using Weekl.Core.Repository.Feed.Abstract;

namespace Weekl.Core.Repository.Feed
{
    public class SourceRepository : BaseRepository, ISourceRepository
    {
        public SourceRepository(string connectionString) : base(connectionString)
        {
        }

        public Source Add(string name, string link, string description, string imageUrl)
        {
            var parameters = new[]
            {
                GetParameter("@name", name),
                GetParameter("@link", link),
                GetParameter("@description", description),
                GetParameter("@imageUrl", imageUrl),
            };

            return Get<Source>("FEED.SourceAdd", CommandType.StoredProcedure, parameters);
        }
        
        public Source Update(int sourceId, string description, string imageUrl)
        {
            var parameters = new[]
            {
                GetParameter("@sourceId", sourceId),
                GetParameter("@description", description),
                GetParameter("@imageUrl", imageUrl)
            };

            return Get<Source>("FEED.SourceUpdate", CommandType.StoredProcedure, parameters);
        }

        public SourceItem Get(int sourceId)
        {
            return Get<SourceItem>("FEED.SourceGet", CommandType.StoredProcedure, GetParameter("@sourceId", sourceId));
        }

        public ICollection<SourceItem> List()
        {
            return GetList<SourceItem>("FEED.SourceList", CommandType.StoredProcedure);
        }

        public void Import(ICollection<SourceXml> sources)
        {
            ExecuteNonQuery("FEED.SourceImportXml", GetParameter("@sources", XmlHelper.SourcesToXml(sources)));
        }
    }
}
