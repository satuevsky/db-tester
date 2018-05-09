using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBTesterLib.Db;

namespace DBTesterUI.Models
{
    class DbItem
    {
        public IDb Db { get; set; }

        private bool _sel;

        public bool Selected
        {
            get => _sel;
            set { _sel = value; }
        }
    }

    class DbTestingConfig
    {
        public List<DbItem> DbList { get; set; }

        public bool Next1Enabled
        {
            get
            {
                return DbList.Any(item => item.Selected);
            }
        }

        public DbTestingConfig()
        {
            DbList = new List<DbItem>();
            DbList.Add(new DbItem { Db = new MongoDb() });
            DbList.Add(new DbItem { Db = new MongoDbSimulator() });
            DbList.Add(new DbItem { Db = new MySqlDb() });
        }
    }
}