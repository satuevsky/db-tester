using System.Collections.Generic;
using System.Linq;
using DBTesterLib.Db;

namespace DBTesterUI.Models.Config
{
    class DbItem
    {
        public IDb Db { get; set; }

        public bool Selected { get; set; }
    }

    class DbItemsModel
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
                new DbItem {Db = new MongoDb()},
                new DbItem {Db = new MongoDbSimulator()},
                new DbItem {Db = new MySqlDb()}
            };
        }
    }
}