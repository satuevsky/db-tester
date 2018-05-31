using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DBTesterLib.Data;
using DBTesterLib.Db;
using DBTesterLib.Tester;
using DBTesterUI.Annotations;
using DBTesterUI.Models.Config.TestModel;
using OxyPlot;

namespace DBTesterUI.Models.Config
{
    class DbTestItem: INotifyPropertyChanged
    {
        public event BaseTester.EventDelegate Progress;

        public string Name { get; set; }
        public IGraphicModel GraphicModel { get; set; }
        public List<DbShardGroup> DbShardGroups { get; set; }
        public BaseTester Tester { get; set; }
        public readonly BaseTester[,] Testers;

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


        public DbTestItem(BaseTester tester, ICollection<DbShardGroup> shardGroups)
        {
            Tester = tester;
            DbShardGroups = shardGroups.ToList();
            Testers = new BaseTester[shardGroups.Count, shardGroups.ElementAt(0).ShardGroupItems.Count];
            GraphicModel = new BarGraphicModel(this);
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
            var db = dbInfo.Db.Create(dbInfo.ConnectionString);
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
            (GraphicModel as IGraphicModel)?.Update();
            Progress?.Invoke();

            OnPropertyChanged(nameof(StateString));
            OnPropertyChanged(nameof(LoadIndicatorVisibility));
            OnPropertyChanged(nameof(LineGraphicModel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class DbTestModel: INotifyPropertyChanged
    {
        public event BaseTester.EventDelegate Progress;

        public List<DbTestItem> Tests { get; set; }

        private DbTestItem _selectedTest;

        public DbTestItem SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        private int _currentItemIndex = 0;

        private List<DbShardGroup> DbShardGroups { get; set; }

        private DbDataModel DataModel { get; set; }

        public DbTestModel()
        {
            Init(new DbShardGroupsModel(), new DbDataModel());
        }

        public DbTestModel(DbShardGroupsModel shardGroupsModel, DbDataModel dataModel)
        {
            Init(shardGroupsModel, dataModel);
        }

        private void Init(DbShardGroupsModel shardGroupsModel, DbDataModel dataModel)
        {
            var data = dataModel.CreateDataSet();
            Tests = new List<DbTestItem>
            {
                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups)
                {
                    Name = "Вставка данных"
                },
                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups)
                {
                    Name = "Выборка данных"
                },
                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups)
                {
                    Name = "Изменение данных"
                },
                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups)
                {
                    Name = "Удаление данных"
                }
            };

            Tests.ForEach(item => { item.Progress += OnProgress; });

            SelectedTest = Tests[0];
        }

        public void Start()
        {
            StartNextTest();
        }

        private void StartNextTest()
        {
            if (_currentItemIndex > Tests.Count - 1)
            {
                MessageBox.Show("Тестирование завершено");
                return;
            }

            switch (Tests[_currentItemIndex].State)
            {
                case TesterState.Stop:
                    Tests[_currentItemIndex].Start();
                    SelectedTest = Tests[_currentItemIndex];
                    break;
                case TesterState.Complete:
                    _currentItemIndex++;
                    StartNextTest();
                    break;
            }
        }

        protected virtual void OnProgress()
        {
            Progress?.Invoke();
            StartNextTest();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}