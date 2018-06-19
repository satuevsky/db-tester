using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DBTesterLib.Data;
using MySql.Data.MySqlClient;

namespace DBTesterLib.Db
{
    public class MySqlDb : IDb
    {
        private string _tableName = "t1";
        private string _dbName = "db1";
        private MySqlConnection _connection;
        private DataColumn[] _columns;


        public string Name => "MySQL";

        public IDb Create(string connectionString, DataColumn[] columns)
        {
            var db = new MySqlDb
            {
                _columns = columns,
                _connection = new MySqlConnection(connectionString)
            };
            db._connection.Open();

            if (db._connection.Database.Length == 0)
            {
                db.SelectDb(_dbName);
            }

            db.PrepareTable(_tableName);
            return db;
        }

        public bool CheckConnectionString(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return true;
        }

        public DataSet Select(PrimaryKeysRange keysRange)
        {
            using (var connection = OpenConnection())
            {
                var query = $"SELECT * FROM {_dbName}.{_tableName} WHERE _id >= @from AND _id <= @to";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.Add("@from", MySqlDbType.Int64).Value = keysRange.From;
                cmd.Parameters.Add("@to", MySqlDbType.Int64).Value = keysRange.To;

                var dataSet = new DataSet(_columns);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var values = new object[_columns.Length];
                    for (int i = 0; i < _columns.Length; i++)
                    {
                        var column = _columns[i];
                        object value = null;
                        switch (column.Type)
                        {
                            case DataType.Number:
                                value = reader.GetInt32(column.Name);
                                break;
                            case DataType.String:
                                value = reader.GetString(column.Name);
                                break;
                            case DataType.Date:
                                value = reader.GetDateTime(column.Name);
                                break;
                            case DataType.Boolean:
                                value = reader.GetBoolean(column.Name);
                                break;
                        }

                        values[i] = value;
                    }

                    dataSet.AddRow(values);
                }

                return dataSet;
            }
        }

        public void Insert(DataSet dataSet)
        {
            var insertQuery = $"INSERT INTO {_dbName}.{_tableName} ({String.Join(",", _columns.Select(c => c.Name))})";
            var values = String.Join(",", dataSet.Rows.Select(row =>
            {
                var rowStr = "";
                for (var i = 0; i < row.Columns.Length; i++)
                {
                    rowStr += ToMySqlValue(row.Values[i], row.Columns[i]);

                    if (i < row.Columns.Length - 1)
                    {
                        rowStr += ",";
                    }
                }

                return $"({rowStr})";
            }));

            insertQuery += $"VALUES {values};";

            using (var connection = (MySqlConnection) _connection.Clone())
            {
                connection.Open();
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(PrimaryKeysRange keysRange, DataRow row)
        {
            var updateQuery = $@"UPDATE {_dbName}.{_tableName} ";
            updateQuery +=
                $"SET {string.Join(",", row.Columns.Select((column, i) => ToMySqlValue(row.Values[i], column)))} ";
            updateQuery += $"WHERE {_columns[0].Name} >= @from AND {_columns[0].Name} <= @to;";

            using (var connection = (MySqlConnection) _connection.Clone())
            {
                connection.Open();
                var cmd = new MySqlCommand(updateQuery, connection);
                cmd.Parameters.Add("@from", MySqlDbType.Int64).Value = keysRange.From;
                cmd.Parameters.Add("@to", MySqlDbType.Int64).Value = keysRange.To;
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(PrimaryKeysRange keysRange)
        {
            var deleteQuery = $@"DELETE FROM {_dbName}.{_tableName} WHERE {_columns[0].Name} >= @from AND {_columns[0].Name} <= @to;";

            using (var connection = (MySqlConnection)_connection.Clone())
            {
                connection.Open();
                var cmd = new MySqlCommand(deleteQuery, connection);
                cmd.Parameters.Add("@from", MySqlDbType.Int64).Value = keysRange.From;
                cmd.Parameters.Add("@to", MySqlDbType.Int64).Value = keysRange.To;
                cmd.ExecuteNonQuery();
            }
        }

        private void SelectDb(string dbName)
        {
            var q = $"CREATE DATABASE IF NOT EXISTS {dbName};";
            var cmd = new MySqlCommand(q, _connection);
            cmd.ExecuteNonQuery();
        }

        private void PrepareTable(string tableName)
        {
            var dropQuery = $"DROP TABLE IF EXISTS {_dbName}.{tableName};";

            var columnsString = String.Join("", _columns.Select((column, i) =>
            {
                string colType;
                switch (column.Type)
                {
                    case DataType.Number:
                        colType = "INT";
                        break;
                    case DataType.String:
                        colType = "TEXT";
                        break;
                    case DataType.Date:
                        colType = "DATE";
                        break;
                    case DataType.Boolean:
                        colType = "BIT";
                        break;
                    default:
                        colType = "";
                        break;
                }

                return $"{column.Name} {colType},";
            })) + $"PRIMARY KEY({_columns[0].Name})";

            var createQuery = $"CREATE TABLE {_dbName}.{tableName} ({columnsString});";


            var cmd = new MySqlCommand(dropQuery + createQuery, _connection);
            cmd.ExecuteNonQuery();
        }

        private MySqlConnection OpenConnection()
        {
            var connection = (MySqlConnection) _connection.Clone();
            connection.Open();
            return connection;
        }

        private string ToMySqlValue(object val, DataColumn column)
        {
            string valStr;
            switch (column.Type)
            {
                case DataType.Number:
                    valStr = val.ToString();
                    break;
                case DataType.String:
                    valStr = $"'{val}'";
                    break;
                case DataType.Date:
                    valStr = $"'{((DateTime) val):yyyy-MM-dd HH:mm:ss}'";
                    break;
                case DataType.Boolean:
                    valStr = ((bool) val) ? "1" : "0";
                    break;
                default:
                    throw new Exception("Unknown column's type");
            }

            return valStr;
        }
    }
}