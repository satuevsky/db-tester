using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DBTesterLib.Data;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    public class InsertionTester : BaseTester
    {
        private readonly IEnumerable<DataSet> _data;

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Датасет для вставки</param>
        public InsertionTester(DataSet data)
        {
            this._data = new[] {data};
        }

        /// <summary>
        /// Создает экземпляр тестера для измерения времени вставки данных
        /// </summary>
        /// <param name="db">База данных</param>
        /// <param name="data">Массив датасетов для вставки</param>
        public InsertionTester(IEnumerable<DataSet> data)
        {
            this._data = data;
        }

        public override BaseTester Create(IDb db)
        {
            return new InsertionTester(this._data) {Database = db};
        }

        /// <summary>
        /// Метод для 
        /// </summary>
        protected override void Test()
        {
            int length = _data.Count();

            int i = 0;
            int threadCount = 1;
            object lockObj = new Object();

            for (var ti = 0; ti < threadCount; ti++)
            {
                new Thread(() =>
                {
                    DataSet dataSet;
                    while (true)
                    {
                        lock (lockObj)
                        {
                            if (i >= length)
                            {
                                OnProgress(1);
                                break;
                            }
                            dataSet = _data.ElementAt(i++);
                        }

                        var startTime = DateTime.Now;
                        Database.Insert(dataSet);
                        var elapsedTime = DateTime.Now - startTime;
                        double timeForOneRow = elapsedTime.TotalMilliseconds / dataSet.Rows.Count;
                        OnSpeed(timeForOneRow);
                        OnProgress((double) i / length);
                    }
                }).Start();
            }
        }
    }
}