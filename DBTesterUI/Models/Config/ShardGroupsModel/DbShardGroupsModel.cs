using System.Collections.ObjectModel;
using System.Linq;

namespace DBTesterUI.Models.Config.ShardGroupsModel
{
    class DbShardGroupsModel
    {
        private readonly DbItemsModel _dbItemsModel;

        /// <summary>
        /// Коллекция групп баз данных.
        /// </summary>
        public ObservableCollection<DbShardGroup> ShardGroups { get; set; }

        /// <summary>
        /// Конструктор модели.
        /// </summary>
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

        /// <summary>
        /// Конструктор модели на основе модели списка бд.
        /// </summary>
        /// <param name="dbItems"></param>
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

        /// <summary>
        /// Метод для добаления группы в модель.
        /// </summary>
        public void AddGroup()
        {
            ShardGroups.Add(new DbShardGroup(_dbItemsModel.SelectedItems.Select(item => item.Db).ToList())
            {
                MachinesCount = 1
            });
        }

        /// <summary>
        /// Метод для удаления группы из модели.
        /// </summary>
        /// <param name="group">Группа для удаления</param>
        public void RemoveGroup(DbShardGroup group)
        {
            ShardGroups.Remove(group);
        }
    }
}