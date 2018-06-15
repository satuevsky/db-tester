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

        public IDb Create(string connectionString, DataColumn[] columns)
        {
            var db = new MongoDbSimulator
            {
                _timeout = int.Parse(connectionString)
            };
            if (db._timeout < 1) db._timeout = 1;
            return db;
        }

        public bool CheckConnectionString(string connectionString)
        {
            Thread.Sleep(100);
            return true;
        }

        public DataSet Select(PrimaryKeysRange keysRange)
        {
            Thread.Sleep(_rand.Next(1, _timeout));
            return new DataSet(new DataColumn[0]);
        }

        public void Insert(DataSet dataSet)
        {
            Thread.Sleep(_rand.Next(1, _timeout));
        }

        public void Update(PrimaryKeysRange keysRange, DataRow row)
        {
            Thread.Sleep(_rand.Next(1, _timeout));
        }

        public void Delete(PrimaryKeysRange keysRange)
        {
            Thread.Sleep(_rand.Next(1, _timeout));
        }
    }
}