using System.Collections.Generic;

namespace DBTesterLib.Data
{
    public class DataSet
    {
        private readonly Dictionary<string, DataType> _columnsParam;

        /// <summary>
        /// Стобцы данных
        /// </summary>
        public DataColumn[] Columns { get; private set; }
        
        /// <summary>
        /// Строки данных
        /// </summary>
        public List<DataRow> Rows { get; private set; }

        /// <summary>
        /// Конструктор набора данных
        /// </summary>
        /// <param name="columns">Столбцы</param>
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

        /// <summary>
        /// Конструктор набора данных
        /// </summary>
        /// <param name="columns">Столбцы</param>
        public DataSet(DataColumn[] columns)
        {
            this.Columns = columns;
            this.Rows = new List<DataRow>();
        }

        /// <summary>
        /// Метод для добавления строки
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataRow AddRow(object[] values)
        {
            var row = new DataRow(values, this.Columns);
            this.Rows.Add(row);
            return row;
        }

        /// <summary>
        /// Метод для получения части набора данных
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
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
