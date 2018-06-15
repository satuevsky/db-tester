using System;
using System.Collections.Generic;
using System.Windows.Media.Converters;
using DBTesterLib.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DBTesterLib.Db
{
    public class MongoDb : IDb
    {
        public string Name => "MongoDB";

        private IMongoCollection<BsonDocument> _collection;
        private readonly string _dbName = "db1";
        private readonly string _collectionName = "t1";
        private DataColumn[] _columns;

        public IDb Create(string connectionString, DataColumn[] columns)
        {
            var db = new MongoDb();
            var m = new MongoClient(connectionString);
            var mdb = m.GetDatabase(_dbName);
            //mdb.DropCollection(_collectionName);
            db._collection = mdb.GetCollection<BsonDocument>(_collectionName);

            db._collection.DeleteMany(Builders<BsonDocument>.Filter.Empty);

            db._columns = columns;
            return db;
        }

        public bool CheckConnectionString(string connectionString)
        {
            //return connectionString.StartsWith("mongodb");
            try
            {
                new MongoClient(connectionString).ListDatabases();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataSet Select(PrimaryKeysRange keysRange)
        {
            var filter = Builders<BsonDocument>.Filter.Gte("_id", keysRange.From) &
                         Builders<BsonDocument>.Filter.Lte("_id", keysRange.To);

            var docs = _collection.FindSync(filter).ToEnumerable();
            var result = new DataSet(_columns);

            foreach (var doc in docs)
            {
                var values = new object[doc.ElementCount];

                for (int i = 0; i < _columns.Length; i++)
                {
                    var column = _columns[i];
                    object value;

                    switch (column.Type)
                    {
                        case DataType.Number:
                            value = doc.GetValue(column.Name).AsInt32;
                            break;
                        case DataType.String:
                            value = doc.GetValue(column.Name).AsString;
                            break;
                        case DataType.Boolean:
                            value = doc.GetValue(column.Name).AsBoolean;
                            break;
                        case DataType.Date:
                            value = doc.GetValue(column.Name).AsBsonDateTime.ToUniversalTime();
                            break;
                        default:
                            value = null;
                            break;
                    }

                    values[i] = value;
                }
                result.AddRow(values);
            }

            return result;
        }

        public void Insert(DataSet dataSet)
        {
            var documents = new BsonDocument[dataSet.Rows.Count];
            var columns = dataSet.Columns;

            for (var i = 0; i < documents.Length; i++)
            {
                var row = dataSet.Rows[i];
                var fields = new BsonElement[columns.Length];

                for (var fi = 0; fi < columns.Length; fi++)
                {
                    fields[fi] = new BsonElement(columns[fi].Name, BsonValue.Create(row.Values[fi]));
                }

                documents[i] = new BsonDocument((IEnumerable<BsonElement>) fields);
            }

            _collection.InsertMany(documents);
        }

        public void Update(PrimaryKeysRange keysRange, DataRow row)
        {
            var filter = Builders<BsonDocument>.Filter.Gte("_id", keysRange.From) &
                         Builders<BsonDocument>.Filter.Lte("_id", keysRange.To);

            var updates = new UpdateDefinition<BsonDocument>[row.Columns.Length - 1];

            for (var i = 0; i < row.Columns.Length; i++)
            {
                var column = row.Columns[i];
                if (column.Name != "_id")
                {
                    updates[i] = Builders<BsonDocument>.Update.Set(column.Name, row.Values[i]);
                }
            }

            _collection.UpdateMany(filter, Builders<BsonDocument>.Update.Combine(updates));
        }

        public void Delete(PrimaryKeysRange keysRange)
        {
            var filter = Builders<BsonDocument>.Filter.Gte("_id", keysRange.From) &
                         Builders<BsonDocument>.Filter.Lte("_id", keysRange.To);

            _collection.DeleteMany(filter);
        }
    }
}