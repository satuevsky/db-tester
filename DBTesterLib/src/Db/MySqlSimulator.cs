using System;
using System.Threading;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public class MySqlSimulator : IDb
    {
        public string Name => "MySQL(Simulator)";

        private int _timeout;
        private Random _rand;

        public MySqlSimulator()
        {
            _rand = new Random();
        }

        public IDb Create(string connectionString, DataColumn[] columns)
        {
            var db = new MySqlSimulator
            {
                _timeout = int.Parse(connectionString)
            };
            if (db._timeout < 1) db._timeout = 1;
            return db;
        }

        public bool CheckConnectionString(string connectionString)
        {
            Thread.Sleep(500);
            return true;
        }

        public DataSet Select(PrimaryKeysRange keysRange)
        {
            Thread.Sleep(1);
            return new DataSet(new DataColumn[0]);
        }

        public void Insert(DataSet dataSet)
        {
            Thread.Sleep(_rand.Next(1, _timeout));
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