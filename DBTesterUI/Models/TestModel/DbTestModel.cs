using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DBTesterLib.Tester;
using DBTesterUI.Annotations;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Models.TestModel
{
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
                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups, data[0].Columns)
                {
                    Name = "Вставка данных"
                },
                new DbTestItem(new SelectionTester(data), shardGroupsModel.ShardGroups, data[0].Columns)
                {
                    Name = "Выборка данных"
                },
//                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups, data[0].Columns)
//                {
//                    Name = "Изменение данных"
//                },
//                new DbTestItem(new InsertionTester(data), shardGroupsModel.ShardGroups, data[0].Columns)
//                {
//                    Name = "Удаление данных"
//                }
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