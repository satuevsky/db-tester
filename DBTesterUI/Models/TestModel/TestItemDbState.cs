using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using DBTesterUI.Annotations;

namespace DBTesterUI.Models.TestModel
{
    class TestItemDbState: INotifyPropertyChanged
    {
        private DbTestItem _testItem;
        private int _groupIndex;
        private int _dbIndex;

        public string Name => "[" + _testItem.DbShardGroups[_groupIndex].MachinesCount + "]" +_testItem.DbShardGroups[_groupIndex].ShardGroupItems[_dbIndex].Db.Name;

        public string Duration
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double duration = 0;
                if (tester != null && tester.Duration.Seconds > 0)
                {
                    duration = tester.Duration.Seconds;
                }

                return DoubleToString(duration) + " сек";
            }
        }

        public string RowsInSecond
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null && tester.Speed > 0)
                {
                    rowsInSecond = tester.Speed;
                }

                return DoubleToString(rowsInSecond) + " зап/сек";
            }
        }

        public string RowsInSecondAvg
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null && tester.AvgSpeed > 0)
                {
                    rowsInSecond = tester.AvgSpeed;
                }

                return DoubleToString(rowsInSecond) + " зап/сек";
            }
        }

        private string DoubleToString(double val)
        {
            string str = val.ToString("##.###");
            return str.Length == 0 ? "0" : str;
        }


        public TestItemDbState(DbTestItem testItem, int groupIndex, int dbIndex)
        {
            _testItem = testItem;
            _groupIndex = groupIndex;
            _dbIndex = dbIndex;

            var t = new Timer(1000);

            t.Elapsed += (sender, args) =>
            {
                OnPropertyChanged(nameof(RowsInSecond));
                OnPropertyChanged(nameof(RowsInSecondAvg));
                OnPropertyChanged(nameof(Duration));
            };

            t.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}