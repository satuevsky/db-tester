using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    public class SelectionTester : BaseTester
    {
        public SelectionTester(IEnumerable<DataSet> data) : base(data)
        {
        }

        public override BaseTester Create(IDb db)
        {
            return new SelectionTester(DataSets) {Database = db};
        }

        protected override void Test(DataSet dataSet)
        {
            var keysRange = new PrimaryKeysRange(dataSet.Rows.First().Values[0], dataSet.Rows.Last().Values[0]);
            Database.Select(keysRange);
        }
    }
}