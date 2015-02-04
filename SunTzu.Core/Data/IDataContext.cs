using System.Collections.Generic;

namespace SunTzu.Core.Data
{
    public interface IDataContext
    {
        void Delete(IEntity data);
        void DeleteAll<T>(IEnumerable<T> dataList);
        void Insert(IEntity data);
        void InsertAll<T>(IEnumerable<T> dataList);

        IEnumerable<T> ExecuteQuery<T>(string queryString, params object[] parameters);
        int ExecuteCommand(string sqlCommand);
        int SaveChanges();
    }
}
