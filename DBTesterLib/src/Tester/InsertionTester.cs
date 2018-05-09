using System.Collections.Generic;
using System.Linq;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    class InsertionTester: BaseTester
    {
        private readonly IEnumerable<DataSet> _data;

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Датасет для вставки</param>
        public InsertionTester(IDb db, DataSet data): base(db)
        {
            this._data = new []{data};
        }

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Массив датасетов для вставки</param>
        public InsertionTester(IDb db, IEnumerable<DataSet> data) : base(db)
        {
            this._data = data;
        }

        /// <summary>
        /// Метод для 
        /// </summary>
        protected override void Test()
        {
            int length = _data.Count();
            double i = 0;

            foreach (var dataSet in _data)
            {
                Database.Insert(dataSet);
                i++;
                this.OnProgress(i / length);
            }
        }
    }
}