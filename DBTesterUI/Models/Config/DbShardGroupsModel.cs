using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using DBTesterLib.Data;
using DBTesterLib.Db;
using DBTesterUI.Annotations;

namespace DBTesterUI.Models.Config
{
    enum ConnectionStringState
    {
        Valid,
        NotValid,
        NotSet,
        Checking,
    }

    class DbShardGroupItem : INotifyPropertyChanged
    {
        private IDb _initedDb;

        public IDb Db { get; set; }
        public string ConnectionString { get; set; }

        private ConnectionStringState _connectionStringState = ConnectionStringState.NotSet;

        public ConnectionStringState ConnectionStringState
        {
            get => _connectionStringState;
            set
            {
                _connectionStringState = value;
                OnPropertyChanged(nameof(ConnectionStringTextColor));
                OnPropertyChanged(nameof(ConnectionStringCheckButtonText));
            }
        }

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

        public IDb InitDb(DataColumn[] columns)
        {
            return _initedDb ?? (_initedDb = Db.Create(ConnectionString, columns));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class DbShardGroup : INotifyPropertyChanged
    {
        private int _machinesCount = 1;

        public int MachinesCount
        {
            get => _machinesCount;
            set
            {
                _machinesCount = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public List<DbShardGroupItem> ShardGroupItems { get; set; }

        public string Title => "Базы данных с количеством машин: " + MachinesCount;

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