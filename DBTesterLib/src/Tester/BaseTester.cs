using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using DBTesterLib.Data;
using DBTesterLib.Db;
using DBTesterLib.Tester.Utils;

namespace DBTesterLib.Tester
{
    /// <summary>
    /// Базовый абстрактный класс для тестирования производительности баз данных
    /// </summary>
    public abstract class BaseTester
    {
        public delegate void EventDelegate();

        /// <summary>
        /// Событие вызываемое при старте тестирования.
        /// </summary>
        public event EventDelegate Started;

        /// <summary>
        /// Событие вызываемое при изменении прогересса выполнения тестирования.
        /// </summary>
        public event EventDelegate Progress;

        /// <summary>
        /// Событие вызываемое при завершении тестирования.
        /// </summary>
        public event EventDelegate Completed;

        /// <summary>
        /// Объект для работы с бд.
        /// </summary>
        public IDb Database { get; protected set; }

        /// <summary>
        /// Данные для тестирования.
        /// </summary>
        public IEnumerable<DataSet> DataSets { get; set; }

        /// <summary>
        /// Количество потоков тестирования.
        /// </summary>
        public int ThreadsCount { get; set; }

        /// <summary>
        /// Состояние тестера.
        /// </summary>
        public TesterState State { get; private set; }

        /// <summary>
        /// Прогресс выполнения теста. От 0.0 до 1.0.
        /// </summary>
        public double ProgressValue { get; private set; }

        /// <summary>
        /// Время затраченное на выполнение теста.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (State == TesterState.Stop) return TimeSpan.Zero;
                if (State == TesterState.InProgress) return DateTime.Now - _starTime;
                return _completeTime - _starTime;
            }
        }

        /// <summary>
        /// Текущее время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double Speed => _speedLogger.Current;

        /// <summary>
        /// Среднее время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double AvgSpeed => _speedLogger.Avg;

        private DateTime _starTime;
        private DateTime _completeTime;
        private SpeedLogger _speedLogger;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="dataSets">Данные для тестирования</param>
        protected BaseTester(IEnumerable<DataSet> dataSets)
        {
            ProgressValue = 0;
            ThreadsCount = 100;
            State = TesterState.Stop;
            DataSets = dataSets;
            _speedLogger = new SpeedLogger(1000);
        }

        /// <summary>
        /// Метод для создания нового экземпляра теста.
        /// </summary>
        /// <param name="db">бд</param>
        /// <returns></returns>
        public abstract BaseTester Create(IDb db);

        /// <summary>
        /// Метод в котором необходимо выполнять действия,
        /// производительность которых будет измеряться
        /// </summary>
        protected abstract void Test(DataSet dataSet);

        /// <summary>
        /// Метод для запуска тестирования
        /// </summary>
        public void Start()
        {
            if (State != TesterState.Stop) return;
            OnStart();

            object
                lockObj1 = new object(),
                lockObj2 = new object();

            int dataSetsLength = DataSets.Count(),
                nextDataSetIndex = 0,
                completedDataSets = 0;

            for (var ti = 0; ti < ThreadsCount; ti++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        DataSet dataSet;
                        lock (lockObj1)
                        {
                            if (nextDataSetIndex >= dataSetsLength)
                            {
                                break;
                            }

                            dataSet = DataSets.ElementAt(nextDataSetIndex++);
                        }

                        Test(dataSet);
                        _speedLogger.Log(dataSet.Rows.Count);

                        lock (lockObj2)
                        {
                            ++completedDataSets;
                        }

                        OnProgress((double) completedDataSets / dataSetsLength);
                    }
                }).Start();
            }
        }

        /// <summary>
        /// Метод для вызова события Start
        /// </summary>
        private void OnStart()
        {
            this._starTime = DateTime.Now;
            this.State = TesterState.InProgress;
            this.Started?.Invoke();
        }

        /// <summary>
        /// Метод для вызова события Complete
        /// </summary>
        private void OnComplete()
        {
            if (State == TesterState.Complete) return;

            this._speedLogger.Stop();
            this._completeTime = DateTime.Now;
            this.State = TesterState.Complete;
            this.Completed?.Invoke();
        }

        /// <summary>
        /// Метод для вызова события Progress
        /// </summary>
        private void OnProgress(double progressValue)
        {
            ProgressValue = progressValue;
            this.Progress?.Invoke();
            if (progressValue >= 1)
            {
                OnComplete();
            }
        }
    }
}