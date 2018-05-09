using System.Collections.Generic;
using DBTesterLib.Db;

namespace DBTesterUI.Models
{
    class DbShards
    {
        /// <summary>
        /// Название связки баз данных.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Словарь баз данных, в котором ключ это количество машин, на которых
        /// распределенно работает БД, а значение это интерфейс для работы с БД.
        /// </summary>
        public Dictionary<int, IDb> Databases { get; private set; }


        /// <param name="name">Название базы данных</param>
        public DbShards(string name)
        {
            Name = name;
            Databases = new Dictionary<int, IDb>();
        }


        /// <summary>
        /// Метод для указания базы данных с соответствующим количеством машин.
        /// </summary>
        /// <param name="machinesNumber">Количество машин базы данных.</param>
        /// <param name="db">Интерфейс для работы с БД.</param>
        public void SetShard(int machinesNumber, IDb db)
        {
            Databases.Add(machinesNumber, db);
        }
    }
}