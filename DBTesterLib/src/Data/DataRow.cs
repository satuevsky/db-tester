using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTesterLib.Data
{
    public class DataRow
    {
        public DataColumn[] Columns { get; set; }
        public object[] Values { get; private set; }

        internal DataRow(object[] values, DataColumn[] columns)
        {
            this.Columns = columns;
            this.Values = values;
        }

        public T GetValue<T>(int columnIndex)
        {
            return (T)this.Values[columnIndex];
        }
    }
}
