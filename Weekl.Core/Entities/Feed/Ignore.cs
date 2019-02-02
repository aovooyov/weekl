using System.Data.Common;

namespace Weekl.Core.Entities.Feed
{
    public class Ignore : BaseModel, IBaseModel
    {
        public int Id { get; set; }
        public string Cause { get; set; }
        public int Type { get; set; }

        public void Fill(DbDataReader r)
        {
            Id = Get<int>(r, "Id");
            Cause = Get<string>(r, "Cause");
            Type = Get<int>(r, "Type");
        }
    }
}