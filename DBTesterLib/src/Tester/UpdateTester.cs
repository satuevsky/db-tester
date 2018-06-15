using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    public class UpdateTester : BaseTester
    {
        private DataRow _forUpdateRow;

        public UpdateTester(IEnumerable<DataSet> data) : base(data)
        {
            var columns = data.ElementAt(0).Columns;
            var values = new object[columns.Length];

            for (var i = 0; i < columns.Length; i++)
            {
                object value;
                switch (columns[i].Type)
                {
                    case DataType.Number:
                        value = 777;
                        break;
                    case DataType.String:
                        value = "777";
                        break;
                    case DataType.Date:
                        value = new DateTime(1994, 7, 23);
                        break;
                    case DataType.Boolean:
                        value = true;
                        break;
                    default:
                        value = null;
                        break;
                }

                values[i] = value;
            }

            _forUpdateRow = new DataRow(values, columns);
        }

        public override BaseTester Create(IDb db)
        {
            return new UpdateTester(DataSets) {Database = db};
        }

        protected override void Test(DataSet dataSet)
        {
            var keysRange = new PrimaryKeysRange(dataSet.Rows.First().Values[0], dataSet.Rows.Last().Values[0]);
            Database.Update(keysRange, _forUpdateRow);
        }
    }
}