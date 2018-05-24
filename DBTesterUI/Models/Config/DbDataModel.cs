﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBTesterLib.Data;

namespace DBTesterUI.Models.Config
{
    class DbDataColumn
    {
        public string Name { get; set; }

        public DataType Type { get; set; }
    }

    class DbDataModel
    {
        public int RowsCount { get; set; }

        public List<DbDataColumn> Columns { get; set; }

        public DbDataModel()
        {
            RowsCount = 100000;

            Columns = new List<DbDataColumn>
            {
                new DbDataColumn {Name = "Id", Type = DataType.Number},
                new DbDataColumn {Name = "Name", Type = DataType.String},
                new DbDataColumn {Name = "BDate", Type = DataType.Date},
                new DbDataColumn {Name = "Age", Type = DataType.Number},
            };
        }

        public DataSet CreateDataSet()
        {
            var result = new DataSet(
                Columns.ToDictionary(column => column.Name, column => column.Type)
            );
            var random = new Random();
            long minDate = new DateTime(1900, 1, 1).Ticks;
            long maxDate = DateTime.Now.Ticks;
            
            for (int i = 0; i < RowsCount; i++)
            {
                object[] rowData = new object[this.Columns.Count];

                for (int j = 0; j < Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        // Первичный ключ
                        rowData[j] = i + 1;
                    }
                    else
                    {
                        DbDataColumn column = Columns[j];
                        object value;

                        switch (column.Type)
                        {
                            case DataType.Number:
                                value = random.Next(int.MinValue, int.MaxValue);
                                break;
                            case DataType.Boolean:
                                value = random.Next(0, 1) == 1;
                                break;
                            case DataType.Date:
                                value = RandomDate(minDate, maxDate, random);
                                break;
                            case DataType.String:
                                value = RandomString(32, random);
                                break;
                            default:
                                value = null;
                                break;
                        }

                        rowData[j] = value;
                    }
                }

                result.AddRow(rowData);
            }

            return result;
        }

        private DateTime RandomDate(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            long ticks = (Math.Abs(longRand % (max - min)) + min);
            return new DateTime(ticks);
        }

        private string _randomStringChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private string RandomString(int length, Random rand)
        {
            StringBuilder result = new StringBuilder(length);

            while (length-- > 0)
            {
                result.Append(
                    _randomStringChars[
                        rand.Next(_randomStringChars.Length - 1)
                    ]
                );
            }

            return result.ToString();
        }
    }
}