using System.Collections.ObjectModel;
using System.Linq;

namespace DBTesterUI.Models.Config.ShardGroupsModel
{
    class DbShardGroupsModel
    {
        private readonly DbItemsModel _dbItemsModel;

        public ObservableCollection<DbShardGroup> ShardGroups { get; set; }

        public DbShardGroupsModel()
        {
            _dbItemsModel = new DbItemsModel();
            _dbItemsModel.DbList.ForEach(item => item.Selected = true);
            Init();

            AddGroup();
            ShardGroups[1].MachinesCount = 2;
            AddGroup();
            ShardGroups[2].MachinesCount = 4;
        }

        public DbShardGroupsModel(DbItemsModel dbItems)
        {
            _dbItemsModel = dbItems;
            Init();
        }

        private void Init()
        {
            ShardGroups = new ObservableCollection<DbShardGroup>();
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