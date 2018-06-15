namespace DBTesterLib.Db
{
    public class PrimaryKeysRange
    {
        public object From { get; set; }
        public object To { get; set; }

        public PrimaryKeysRange(){}

        public PrimaryKeysRange(object from, object to)
        {
            From = from;
            To = to;
        }
    }
}