using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    public class SelectionTester : BaseTester
    {
        private readonly IEnumerable<DataSet> _data;

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Датасет для вставки</param>
        public SelectionTester(DataSet data)
        {
            this._data = new[] {data};
        }

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Массив датасетов для вставки</param>
        public SelectionTester(IEnumerable<DataSet> data)
        {
            this._data = data;
        }

        public override BaseTester Create(IDb db)
        {
            return new SelectionTester(this._data) {Database = db};
        }

        /// <summary>
        /// Метод для 
        /// </summary>
        protected override void Test()
        {
            var rand = new Random();
            double length = _data.Count();
            double i = 0;

            foreach (var dataSet in _data)
            {
                var keysRange = new PrimaryKeysRange(dataSet.Rows.First().Values[0], dataSet.Rows.Last().Values[0]);
                Database.Select(keysRange);
                this.OnProgress(++i / length);
            }
        }
    }
}