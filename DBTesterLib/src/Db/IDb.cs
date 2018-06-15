using System;
using DBTesterLib.Data;

namespace DBTesterLib.Db
{
    public interface IDb
    {
        /// <summary>
        /// Название базы данных.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Метод для создания копии объекта, с подключением к бд.
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="columns"></param>
        /// <returns></returns>
        IDb Create(string connectionString, DataColumn[] columns);
        
        /// <summary>
        /// Метод для проверки строки подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <returns></returns>
        bool CheckConnectionString(string connectionString);

        /// <summary>
        /// Метод для выполнения запроса на выборку данных.
        /// </summary>
        /// <param name="keysRange"></param>
        /// <returns></returns>
        DataSet Select(PrimaryKeysRange keysRange);

        /// <summary>
        /// Метод для выполнения запроса на вставку данных.
        /// </summary>
        /// <param name="dataSet"></param>
        void Insert(DataSet dataSet);

        /// <summary>
        /// Метод для выполнения запроса на обновление данных.
        /// </summary>
        /// <param name="keysRange"></param>
        /// <param name="row"></param>
        void Update(PrimaryKeysRange keysRange, DataRow row);

        /// <summary>
        /// Метод для выполнения запроса на удаление данных.
        /// </summary>
        /// <param name="keysRange"></param>
        void Delete(PrimaryKeysRange keysRange);
    }
}
