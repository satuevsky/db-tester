using System;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public class MySqlDb: IDb
    {
        public string Name => "MySQL";
        
        public IDb Create(string connectionString)
        {
            throw new NotImplementedException();
        }

        public bool CheckConnectionString(string connectionString)
        {
            return false;
        }

        public void DeleteMany(string primaryKeyName, object[] primaryKeyValues)
        {
            throw new NotImplementedException();
        }

        public void DeleteOne(string primaryKeyName, object primaryKeyValue)
        {
            throw new NotImplementedException();
        }

        public void Insert(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public DataSet SelectMany(string primaryKeyName, object[] primaryKeyValues)
        {
            throw new NotImplementedException();
        }

        public DataSet SelectOne(string primaryKeyName, object primaryKeyValue)
        {
            throw new NotImplementedException();
        }
    }
}
