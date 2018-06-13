using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using DBTesterLib.Data;

namespace DBTesterUI.Models.Config.DataModel
{
    class DbDataModel
    {
        public int RowsCount { get; set; }

        public ObservableCollection<DbDataColumn> Columns { get; set; }

        public ValidationRule ColumnValidationRule => new DbDataColumnValidationRule();

        public int ButchSize { get; set; }


        public DbDataModel()
        {
            RowsCount = 50000;
            ButchSize = 500;

            Columns = new ObservableCollection<DbDataColumn>
            {
                new DbDataColumn {Name = "_id", Type = DataType.Number, IsPrimary = true},
                new DbDataColumn {Name = "name", Type = DataType.String},
                new DbDataColumn {Name = "bdate", Type = DataType.Date},
                new DbDataColumn {Name = "age", Type = DataType.Number},
            };
        }

        public List<DataSet> CreateDataSet()
        {
            Columns = new ObservableCollection<DbDataColumn>(Columns.Where(column => column.IsValid()));

            var result = new List<DataSet>((int)Math.Ceiling((decimal)RowsCount / ButchSize));
            var dataSet = new DataSet(
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
                                value = RandomString(16, random);
                                break;
                            default:
                                value = null;
                                break;
                        }

                        rowData[j] = value;
                    }
                }

                dataSet.AddRow(rowData);
            }

            int copied = 0;
            while (copied < dataSet.Rows.Count)
            {
                result.Add(dataSet.Slice(copied, ButchSize));
                copied += ButchSize;
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