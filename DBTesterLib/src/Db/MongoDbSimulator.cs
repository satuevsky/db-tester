using System;
using System.Threading;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    class MongoDbSimulator: IDb
    {
        public string Name => "MongoDb(Simulator)";


        private readonly int _timeout;

        public MongoDbSimulator(int timeout)
        {
            this._timeout = timeout;
        }


        public void Insert(DataSet dataSet)
        {
            Thread.Sleep(_timeout + new Random(DateTime.Now.Millisecond).Next(-1000, 1000));
        }

        public DataSet SelectOne(string primaryKeyName, object primaryKeyValue)
        {
            Thread.Sleep(new Random(DateTime.Now.Millisecond).Next());
            return null;
        }

        public DataSet SelectMany(string primaryKeyName, object[] primaryKeyValues)
        {
            Thread.Sleep(new Random(DateTime.Now.Millisecond).Next());
            return null;
        }

        public void DeleteOne(string primaryKeyName, object primaryKeyValue)
        {
            Thread.Sleep(new Random(DateTime.Now.Millisecond).Next());
        }

        public void DeleteMany(string primaryKeyName, object[] primaryKeyValues)
        {
            Thread.Sleep(new Random(DateTime.Now.Millisecond).Next());
        }
    }
}
