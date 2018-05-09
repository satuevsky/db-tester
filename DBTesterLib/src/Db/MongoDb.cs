using System;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    class MongoDb: IDb
    {
        public string Name => "MongoDB";

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
