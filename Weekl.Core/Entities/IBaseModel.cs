using System.Data.Common;

namespace Weekl.Core.Entities
{
    public interface IBaseModel
    {
        void Fill(DbDataReader r);
    }
}
