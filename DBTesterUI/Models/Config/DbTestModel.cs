using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DBTesterLib.Data;
using DBTesterLib.Db;
using DBTesterLib.Tester;
using DBTesterUI.Models.Config.TestModel;

namespace DBTesterUI.Models.Config
{
    class DbTestItem
    {
        public string Name { get; set; }

        public BaseTester Tester { get; set; }
        
        public List<DbShardGroup> DbShardGroups { get; set; }

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

        public readonly BaseTester[,] Testers;

        public GraphicModel GraphicModel { get; set; }
        


        public DbTestItem(BaseTester tester, List<DbShardGroup> shardGroups)
        {
            Tester = tester;
            DbShardGroups = shardGroups;
            Testers = new BaseTester[shardGroups.Count, shardGroups[0].ShardGroupItems.Count];
            GraphicModel = new GraphicModel(this);
        }
    }

    class DbTestModel
    {
        public List<DbTestItem> Tests { get; set; }

        public DbTestItem SelectedTest { get; set; }

        private List<DbShardGroup> DbShardGroups { get; set; }

        private DbDataModel DataModel { get; set; }

        public DbTestModel()
        {
            Tests = new List<DbTestItem>();

            Tests = new List<DbTestItem>
            {
                new DbTestItem(new InsertionTester(new DataSet[0]), new List<DbShardGroup>(){ new DbShardGroup(new List<IDb>{new MongoDb()})})
                {
                    Name = "Вставка данных"
                },
                new DbTestItem(new InsertionTester(new DataSet[0]), new List<DbShardGroup>(){ new DbShardGroup(new List<IDb>{new MongoDb()})})
                {
                    Name = "Выборка данных"
                },
                new DbTestItem(new InsertionTester(new DataSet[0]), new List<DbShardGroup>(){ new DbShardGroup(new List<IDb>{new MongoDb()})})
                {
                    Name = "Изменение данных"
                },
                new DbTestItem(new InsertionTester(new DataSet[0]), new List<DbShardGroup>(){ new DbShardGroup(new List<IDb>{new MongoDb()})})
                {
                    Name = "Удаление данных"
                }
            };

            SelectedTest = Tests[0];
        }

        public DbTestModel(List<DbShardGroup> dbShardGroups, DbDataModel dataModel)
        {
            DbShardGroups = dbShardGroups;
            DataModel = dataModel;

            Tests = new List<DbTestItem>
            {
                new DbTestItem(new InsertionTester(dataModel.CreateDataSet()), dbShardGroups)
                {
                    Name = "Вставка данных"
                },
                new DbTestItem(new InsertionTester(dataModel.CreateDataSet()), dbShardGroups)
                {
                    Name = "Выборка данных"
                },
                new DbTestItem(new InsertionTester(dataModel.CreateDataSet()), dbShardGroups)
                {
                    Name = "Изменение данных"
                },
                new DbTestItem(new InsertionTester(dataModel.CreateDataSet()), dbShardGroups)
                {
                    Name = "Удаление данных"
                }
            };

            SelectedTest = Tests[0];
        }
    }
}