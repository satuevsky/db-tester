using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using DBTesterLib.Db;

namespace DBTesterUI.Models.Config
{
    enum ConnectionStringState
    {
        Valid,
        NotValid,
        NotSet,
        Checking,
    }

    class DbShardGroupItem
    {
        public IDb Db { get; set; }
        public string ConnectionString { get; set; }
        public ConnectionStringState ConnectionStringState { get; set; }

        public Brush ConnectionStringTextColor
        {
            get
            {
                switch (ConnectionStringState)
                {
                    case ConnectionStringState.Checking:
                        return new SolidColorBrush(Color.FromRgb(150, 150, 150));
                    case ConnectionStringState.NotValid:
                        return new SolidColorBrush(Color.FromRgb(150, 0, 0));
                    case ConnectionStringState.Valid:
                        return new SolidColorBrush(Color.FromRgb(0, 150, 0));
                }

                return new SolidColorBrush(Color.FromRgb(171, 173, 179));
            }
        }

        public string ConnectionStringCheckButtonText
        {
            get
            {
                switch (ConnectionStringState)
                {
                    case ConnectionStringState.Checking:
                        return "Проверка...";
                    case ConnectionStringState.NotValid:
                        return "Ошибка";
                    case ConnectionStringState.Valid:
                        return "Ок";
                }

                return "Проверить";
            }
        }

        public DbShardGroupItem(IDb db)
        {
            Db = db;
            ConnectionStringState = ConnectionStringState.NotSet;
        }
    }

    class DbShardGroup
    {
        public int MachinesCount { get; set; }

        public List<DbShardGroupItem> ShardGroupItems { get; set; }

        public string Title => "Базы данных с количеством машин: " + MachinesCount;

        public DbShardGroup(List<IDb> dbs)
        {
            ShardGroupItems = new List<DbShardGroupItem>();
            dbs.ForEach(db => ShardGroupItems.Add(new DbShardGroupItem(db)));
        }
    }

    class DbShardGroupsModel
    {
        private readonly DbItemsModel _dbItemsModel;

        public List<DbShardGroup> ShardGroups { get; set; }

        public DbShardGroupsModel()
        {
            _dbItemsModel = new DbItemsModel();
            _dbItemsModel.DbList.ForEach(item => item.Selected = true);
            Init();
        }

        public DbShardGroupsModel(DbItemsModel dbItems)
        {
            _dbItemsModel = dbItems;
            Init();
        }

        private void Init()
        {
            ShardGroups = new List<DbShardGroup>();
            AddGroup();
        }

        public void AddGroup()
        {
            ShardGroups.Add(new DbShardGroup(_dbItemsModel.SelectedItems.Select(item => item.Db).ToList())
            {
                MachinesCount = 1
            });
        }

        public void RemoveGroup(DbShardGroup group)
        {
            ShardGroups.Remove(group);
        }
    }
}