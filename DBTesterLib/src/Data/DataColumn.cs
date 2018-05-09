namespace DBTesterLib.Data
{
    public class DataColumn
    {
        public string Name { get; private set; }
        public DataType Type { get; private set; }


        internal DataColumn(string name, DataType dataType)
        {
            
            this.Name = name;
            this.Type = dataType;
        }
    }
}
