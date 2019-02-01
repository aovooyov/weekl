using System.Collections.Generic;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Models;

namespace Weekl.Core.Repository.Feed.Abstract
{
    public interface ISourceRepository
    {
        Source Add(string name, string link, string description, string imageUrl);
        Source Update(int sourceId, string description, string imageUrl);
        SourceItem Get(int sourceId);
        ICollection<SourceItem> List();
        void Import(ICollection<SourceXml> sources);
    }
}