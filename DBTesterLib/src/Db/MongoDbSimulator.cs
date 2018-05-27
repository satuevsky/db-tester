using System;
using System.Threading;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public class MongoDbSimulator : IDb
    {
        public string Name => "MongoDb(Simulator)";

        private int _timeout;
        private Random _rand;

        public MongoDbSimulator()
        {
            _rand = new Random();
        }


        public IDb Create(string connectionString)
        {
            var db = new MongoDbSimulator
            {
                _timeout = int.Parse(connectionString)
            };
            if (db._timeout < 100) db._timeout = 100;
            return db;
        }

        public bool CheckConnectionString(string connectionString)
        {
            Thread.Sleep(500);
            return true;
        }

        public void Insert(DataSet dataSet)
        {
            Thread.Sleep(_rand.Next(_timeout));
        }

        public DataSet SelectOne(string primaryKeyName, object primaryKeyValue)
        {
            Thread.Sleep(_rand.Next(_timeout));
            return null;
        }

        public DataSet SelectMany(string primaryKeyName, object[] primaryKeyValues)
        {
            Thread.Sleep(_rand.Next(_timeout));
            return null;
        }

        public void DeleteOne(string primaryKeyName, object primaryKeyValue)
        {
            Thread.Sleep(_rand.Next(_timeout));
        }

        public void DeleteMany(string primaryKeyName, object[] primaryKeyValues)
        {
            Thread.Sleep(_rand.Next(_timeout));
        }
    }
}