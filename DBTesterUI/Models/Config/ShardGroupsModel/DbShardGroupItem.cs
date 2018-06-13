using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using DBTesterLib.Data;
using DBTesterLib.Db;
using DBTesterUI.Annotations;

namespace DBTesterUI.Models.Config.ShardGroupsModel
{
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
}