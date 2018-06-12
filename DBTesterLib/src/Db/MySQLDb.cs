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

        public IDb Create(string connectionString, DataColumn[] columns)
        {
            throw new NotImplementedException();
        }

        public bool CheckConnectionString(string connectionString)
        {
            return true;
        }

        public DataSet Select(PrimaryKeysRange keysRange)
        {
            throw new NotImplementedException();
        }

        public void Insert(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void Update(PrimaryKeysRange keysRange, DataRow row)
        {
            throw new NotImplementedException();
        }

        public void Delete(PrimaryKeysRange keysRange)
        {
            throw new NotImplementedException();
        }
    }
}
