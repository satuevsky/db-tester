using System.Collections.Generic;

namespace DBTesterLib.Data
{
    public class DataSet
    {
        private readonly Dictionary<string, DataType> _columnsParam;

        public DataColumn[] Columns { get; private set; }
        public List<DataRow> Rows { get; private set; }



        public DataSet(Dictionary<string, DataType> columns)
        {
            this._columnsParam = columns;
            this.Columns = new DataColumn[columns.Count];
            this.Rows = new List<DataRow>();
            var i = 0;

            foreach (var columnNameType in columns)
            {
                this.Columns[i++] = new DataColumn(columnNameType.Key, columnNameType.Value);
            }
        }
        public DataSet(DataColumn[] columns)
        {
            this.Columns = columns;
            this.Rows = new List<DataRow>();
        }



        public DataRow AddRow(object[] values)
        {
            var row = new DataRow(values, this.Columns);
            this.Rows.Add(row);
            return row;
        }

        public DataSet Slice(int index, int count)
        {
            var sliceSet = new DataSet(Columns)
            {
                Rows = this.Rows.GetRange(index, count)
            };
            return sliceSet;
        }
    }
}
