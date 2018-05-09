using System.Collections.Generic;

namespace DBTesterUI.Models
{
    class DbTestingModel
    {
        public List<DbShards> DbShardsList { get; set; }

        public List<DbTestingItem> Tests { get; set; }

        public DbTestingModel()
        {
            DbShardsList = new List<DbShards>();
            Tests = new List<DbTestingItem>();
        }
    }
}
