using DBTesterLib.Db;

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
}