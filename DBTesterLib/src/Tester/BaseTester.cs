using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using DBTesterLib.Db;

namespace DBTesterLib.Tester
{
    /// <summary>
    /// Состояния тестера
    /// </summary>
    public enum TesterState
    {
        Stop,
        InProgress,
        Complete,
    }

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

        private DateTime _starTime;
        private DateTime _completeTime;



        protected BaseTester()
        {
            this.ProgressValue = 0;
            this.State = TesterState.Stop;
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
        protected abstract void Test();

        /// <summary>
        /// Метод для запуска тестирования
        /// </summary>
        public void Start()
        {
            if (State != TesterState.Stop) return;

            new Thread(() =>
            {
                this.OnStart();
                Test();
                this.OnComplete();
            }).Start();
        }



        private void OnStart()
        {
            this._starTime = DateTime.Now;
            this.State = TesterState.InProgress;
            this.Started?.Invoke();
        }

        private void OnComplete()
        {
            this._completeTime = DateTime.Now;
            this.State = TesterState.Complete;
            this.Completed?.Invoke();
        }

        protected void OnProgress(double progressValue)
        {
            ProgressValue = progressValue;
            this.Progress?.Invoke();
        }
    }
}
