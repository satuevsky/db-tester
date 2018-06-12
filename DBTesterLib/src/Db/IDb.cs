using System;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public class PrimaryKeysRange
    {
        public object From { get; set; }
        public object To { get; set; }

        public PrimaryKeysRange(){}

        public PrimaryKeysRange(object from, object to)
        {
            From = from;
            To = to;
        }
    }

    public interface IDb
    {
        string Name { get; }

        IDb Create(string connectionString, DataColumn[] columns);
        
        bool CheckConnectionString(string connectionString);

        DataSet Select(PrimaryKeysRange keysRange);

        void Insert(DataSet dataSet);

        void Update(PrimaryKeysRange keysRange, DataRow row);

        void Delete(PrimaryKeysRange keysRange);
    }
}
