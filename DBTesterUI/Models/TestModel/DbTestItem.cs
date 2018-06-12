using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using DBTesterLib.Data;
using DBTesterLib.Tester;
using DBTesterUI.Annotations;
using DBTesterUI.Models.Config;
using DBTesterUI.Models.TestModel.Graphics;

namespace DBTesterUI.Models.TestModel
{
    class TestItemDbState: INotifyPropertyChanged
    {
        private DbTestItem _testItem;
        private int _groupIndex;
        private int _dbIndex;

        public string Name => "[" + _testItem.DbShardGroups[_groupIndex].MachinesCount + "]" +_testItem.DbShardGroups[_groupIndex].ShardGroupItems[_dbIndex].Db.Name;

        public string RowsInSecond
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null)
                {
                    rowsInSecond = 1000 / tester.Speed;
                }

                return rowsInSecond.ToString("## 'зап/сек'");
            }
        }

        public string RowsInSecondAvg
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null)
                {
                    rowsInSecond = 1000 / tester.AvgSpeed;
                }

                return rowsInSecond.ToString("## 'зап/сек'");
            }
        }

        public string RowsInSecondMin
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null)
                {
                    rowsInSecond = 1000 / tester.MaxSpeed;
                }

                return rowsInSecond.ToString("## 'зап/сек'");
            }
        }

        public string RowsInSecondMax
        {
            get
            {
                var tester = _testItem.Testers[_groupIndex, _dbIndex];
                double rowsInSecond = 0;
                if (tester != null)
                {
                    rowsInSecond = 1000 / tester.MinSpeed;
                }

                return rowsInSecond.ToString("## 'зап/сек'");
            }
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
                OnPropertyChanged(nameof(RowsInSecondMax));
                OnPropertyChanged(nameof(RowsInSecondMin));
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

    class DbTestItem: INotifyPropertyChanged
    {
        public event BaseTester.EventDelegate Progress;

        public string Name { get; set; }

        public IGraphicModel GraphicModel { get; set; }

        public List<DbShardGroup> DbShardGroups { get; set; }

        public BaseTester Tester { get; set; }

        public readonly BaseTester[,] Testers;

        public ObservableCollection<TestItemDbState> TestDbStates { get; set; }

        public TesterState State
        {
            get
            {
                TesterState state = TesterState.Stop;
                bool hasStop = false, hasInProgress = false;

                foreach (var tester in Testers)
                {
                    if (tester == null || tester.State == TesterState.Stop)
                    {
                        hasStop = true;
                    }
                    else if (tester.State == TesterState.InProgress)
                    {
                        hasInProgress = true;
                    }
                }

                if (hasInProgress)
                    return TesterState.InProgress;
                if (hasStop)
                    return TesterState.Stop;

                return TesterState.Complete;
            }
        }

        public string StateString
        {
            get
            {
                switch (State)
                {
                    case TesterState.Stop:
                        return "В очереди";
                    case TesterState.InProgress:
                        return "Выполняется";
                    default:
                        return "Завершен";
                }
            }
        }

        public Visibility LoadIndicatorVisibility =>
            State == TesterState.InProgress ? Visibility.Visible : Visibility.Hidden;

        private DataColumn[] _dataColumns;

        public DbTestItem(BaseTester tester, ICollection<DbShardGroup> shardGroups, DataColumn[] dataColumns)
        {
            _dataColumns = dataColumns;
            Tester = tester;
            DbShardGroups = shardGroups.ToList();
            Testers = new BaseTester[shardGroups.Count, shardGroups.ElementAt(0).ShardGroupItems.Count];
            TestDbStates = new ObservableCollection<TestItemDbState>();
            GraphicModel = new BarGraphicModel(this);

            Init();
        }

        private void Init()
        {
            for (var groupIndex = 0; groupIndex < DbShardGroups.Count; groupIndex++)
            {
                for (var dbIndex = 0; dbIndex < DbShardGroups[groupIndex].ShardGroupItems.Count; dbIndex++)
                {
                    TestDbStates.Add(new TestItemDbState(this, groupIndex, dbIndex));
                }
            }
        }

        public void Start()
        {
            if (Testers[0, 0] == null) // Если до этого не запускался тест
            {
                StartNext();
            }
        }

        private void StartNext()
        {
            for (int groupIndex = 0; groupIndex < Testers.GetLength(0); groupIndex++)
            {
                for (int dbIndex = 0; dbIndex < Testers.GetLength(1); dbIndex++)
                {
                    var tester = Testers[groupIndex, dbIndex];
                    if (tester == null)
                    {
                        tester = Testers[groupIndex, dbIndex] = InitTester(groupIndex, dbIndex);
                    }

                    if (tester.State == TesterState.Stop)
                    {
                        tester.Start();
                        return;
                    }
                }
            }
        }

        private BaseTester InitTester(int groupIndex, int dbIndex)
        {
            var dbInfo = DbShardGroups[groupIndex].ShardGroupItems[dbIndex];
            var db = dbInfo.InitDb(_dataColumns);
            var tester = Tester.Create(db);

            tester.Started += OnProgress;
            tester.Progress += OnProgress;
            tester.Completed += () =>
            {
                OnProgress();
                if (State != TesterState.Complete)
                {
                    StartNext();
                }
            };

            return tester;
        }


        protected virtual void OnProgress()
        {
            GraphicModel?.Update();
            Progress?.Invoke();

            OnPropertyChanged(nameof(StateString));
            OnPropertyChanged(nameof(LoadIndicatorVisibility));
            OnPropertyChanged(nameof(LineGraphicModel));
            OnPropertyChanged(nameof(TestDbStates));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}