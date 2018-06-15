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

        /// <summary>
        /// Объект для работы с бд.
        /// </summary>
        public IDb Db { get; set; }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString { get; set; }

        private ConnectionStringState _connectionStringState = ConnectionStringState.NotSet;

        /// <summary>
        /// Состояние строки подключения
        /// </summary>
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

        /// <summary>
        /// Цвет строки подключения
        /// </summary>
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

        /// <summary>
        /// Текст кнопки для проверки строки подключения.
        /// </summary>
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

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="db"></param>
        public DbShardGroupItem(IDb db)
        {
            Db = db;
            ConnectionStringState = ConnectionStringState.NotSet;
        }

        /// <summary>
        /// Метод для инициализации соедния к бд.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
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