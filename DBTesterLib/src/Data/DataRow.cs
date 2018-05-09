namespace DBTesterLib.Data
{
    public class DataRow
    {
        public DataSet DataSet { get; private set; }
        public object[] Values { get; private set; }

        internal DataRow(DataSet dataSet, object[] values)
        {
            this.DataSet = dataSet;
            this.Values = values;
        }

        public T GetValue<T>(int columnIndex)
        {
            return (T)this.Values[columnIndex];
        }
    }
}
