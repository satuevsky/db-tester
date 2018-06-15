using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBTesterLib.Db;
using DBTesterUI.Annotations;

namespace DBTesterUI.Models.Config.ShardGroupsModel
{
    class DbShardGroup : INotifyPropertyChanged
    {
        private int _machinesCount = 1;

        /// <summary>
        /// Количество машин, на которых развернуты бд.
        /// </summary>
        public int MachinesCount
        {
            get => _machinesCount;
            set
            {
                _machinesCount = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Коллекция баз данных, которые входят в эту группу
        /// </summary>
        public List<DbShardGroupItem> ShardGroupItems { get; set; }

        public string Title => "Базы данных с количеством машин: " + MachinesCount;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dbs"></param>
        public DbShardGroup(List<IDb> dbs)
        {
            ShardGroupItems = new List<DbShardGroupItem>();
            dbs.ForEach(db => ShardGroupItems.Add(new DbShardGroupItem(db)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}