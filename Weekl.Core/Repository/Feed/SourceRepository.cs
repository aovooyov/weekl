using System.Collections.Generic;
using System.Data;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Extensions;
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

        public Source Get(int sourceId)
        {
            return Get<Source>("FEED.SourceGet", CommandType.StoredProcedure, GetParameter("@sourceId", sourceId));
        }

        public IEnumerable<Source> List(params int[] sources)
        {
            var parameters = new[]
            {
                GetParameter("@sources", XmlHelper.ArrayToXml(sources))
            };

            return GetList<Source>("FEED.SourceList", CommandType.StoredProcedure, parameters);
        }

        public void Import(IEnumerable<SourceXml> sources)
        {
            ExecuteNonQuery("FEED.SourceImportXml", GetParameter("@sources", XmlHelper.SourcesToXml(sources)));
        }
    }
}
