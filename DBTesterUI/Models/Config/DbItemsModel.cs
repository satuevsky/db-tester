using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DBTesterLib.Db;
using DBTesterUI.Annotations;

namespace DBTesterUI.Models.Config
{
    class DbItem
    {
        public IDb Db { get; set; }

        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                _model.OnPropertyChanged();
            }
        }

        private DbItemsModel _model;

        public DbItem(DbItemsModel model)
        {
            _model = model;
        }
    }

    class DbItemsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Список баз данных для тестирования
        /// </summary>
        public List<DbItem> DbList { get; set; }

        /// <summary>
        /// Выбранные базы данных
        /// </summary>
        public List<DbItem> SelectedItems => DbList.Where(item => item.Selected).ToList();

        public bool AnySelected => SelectedItems.Count > 0;

        /// <summary>
        /// Конструктор 
        /// </summary>
        public DbItemsModel()
        {
            DbList = new List<DbItem>
            {
                new DbItem(this) {Db = new MongoDb()},
                new DbItem(this) {Db = new MongoDbSimulator()},
                new DbItem(this) {Db = new MySqlDb()},
                new DbItem(this) {Db = new MySqlSimulator()},
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        internal void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AnySelected"));
        }
    }
}