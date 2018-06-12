﻿using System;
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

        /// <summary>
        /// Текущее время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double Speed { get; private set; }

        /// <summary>
        /// Среднее время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double AvgSpeed => _speedCount == 0 ? 0 : _speedSum / _speedCount;


        /// <summary>
        /// Минимальное время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double MinSpeed => double.IsNaN(_minSpeed) ? 0 : _minSpeed;

        /// <summary>
        /// Максимальное время в милисекундах, затрачиваемое на обработку одной записи
        /// </summary>
        public double MaxSpeed => double.IsNaN(_maxSpeed) ? 0 : _maxSpeed;

        private DateTime _starTime;
        private DateTime _completeTime;

        private double _minSpeed = Double.NaN;
        private double _maxSpeed = Double.NaN;
        private double _speedSum = 0;
        private int _speedCount = 0;




        protected BaseTester()
        {
            Speed = 0;
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
            }).Start();
        }


        protected void OnSpeed(double speed)
        {
            Speed = speed;
            if (double.IsNaN(_minSpeed) || speed < _minSpeed)
            {
                if (speed == 0)
                {

                }
                _minSpeed = speed;
            }
            if (double.IsNaN(_maxSpeed) || speed > _maxSpeed)
            {
                _maxSpeed = speed;
            }
            _speedSum += speed;
            _speedCount++;
        }

        private void OnStart()
        {
            this._starTime = DateTime.Now;
            this.State = TesterState.InProgress;
            this.Started?.Invoke();
        }

        private void OnComplete()
        {
            if(State == TesterState.Complete) return;

            this._completeTime = DateTime.Now;
            this.State = TesterState.Complete;
            this.Completed?.Invoke();
        }

        protected void OnProgress(double progressValue)
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
