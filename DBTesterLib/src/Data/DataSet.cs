using System.Collections.Generic;

namespace DBTesterLib.Data
{
    public class DataSet
    {
        private readonly Dictionary<string, DataType> _columnsParam;

        public List<DataColumn> Columns { get; private set; }
        public List<DataRow> Rows { get; private set; }

        public DataSet(Dictionary<string, DataType> columns)
        {
            this._columnsParam = columns;
            this.Columns = new List<DataColumn>(columns.Count);
            this.Rows = new List<DataRow>();

            foreach (var columnNameType in columns)
            {
                this.Columns.Add(new DataColumn(columnNameType.Key, columnNameType.Value));
            }
        }

        public DataRow AddRow(object[] values)
        {
            var row = new DataRow(this, values);
            this.Rows.Add(row);
            return row;
        }

        public DataSet Slice(int index, int count)
        {
            var sliceSet = new DataSet(this._columnsParam)
            {
                Rows = this.Rows.GetRange(index, count)
            };
            return sliceSet;
        }
    }
}
