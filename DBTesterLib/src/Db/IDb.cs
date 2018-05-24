using System;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public interface IDb
    {
        string Name { get; }

        IDb Create(string connectionString);
        
        bool CheckConnectionString(string connectionString);

        void Insert(DataSet dataSet);

        DataSet SelectOne(string primaryKeyName, object primaryKeyValue);

        DataSet SelectMany(string primaryKeyName, object[] primaryKeyValues);

        void DeleteOne(string primaryKeyName, object primaryKeyValue);

        void DeleteMany(string primaryKeyName, object[] primaryKeyValues);
    }
}
