using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    public class InsertionTester : BaseTester
    {
        public InsertionTester(IEnumerable<DataSet> dataSets) : base(dataSets)
        {
        }

        public override BaseTester Create(IDb db)
        {
            return new InsertionTester(DataSets) {Database = db};
        }
        
        protected override void Test(DataSet dataSet)
        {
            Database.Insert(dataSet);
        }
    }
}