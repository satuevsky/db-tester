using System;
using System.Collections.Generic;

namespace DBTesterLib.Tester.Utils
{
    class SpeedLogger
    {
        /// <summary>
        /// Текущая скорость
        /// </summary>
        public double Current { get; private set; }

        /// <summary>
        /// Минимальная скорость
        /// </summary>
        public double Min { get; private set; }

        /// <summary>
        /// Максимальная скорость
        /// </summary>
        public double Max { get; private set; }

        /// <summary>
        /// Средняя скорость
        /// </summary>
        public double Avg { get; private set; }

        /// <summary>
        /// Промежуток времени в милисекундах, по которому измеряется скорость
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// История скоростей
        /// </summary>
        public List<IntervalSpeed> History { get; private set; }
        
        private IntervalSpeed _nextIntervalSpeed;
        private bool _minInited;
        private double _speedSum;

        /// <summary>
        /// Конструктор измерителя скорости
        /// </summary>
        /// <param name="interval">Интервал времени в миллисекундах, по которому будет высчитываться скорость</param>
        public SpeedLogger(int interval)
        {
            Interval = interval;
            History = new List<IntervalSpeed>();
        }

        /// <summary>
        /// Метод для добавления единиц скорости
        /// </summary>
        /// <param name="count">Количество единиц скорости</param>
        public void Log(double count)
        {
            if (_nextIntervalSpeed == null)
            {
                _nextIntervalSpeed = new IntervalSpeed(DateTime.Now, 0);
            }

            while (_nextIntervalSpeed.Time.AddMilliseconds(Interval) < DateTime.Now)
            {
                PushHistory();
            }

            _nextIntervalSpeed.Speed += count;
        }


        private void PushHistory()
        {
            double currentSpeed = Current;
            double nextSpeed = currentSpeed + (_nextIntervalSpeed.Speed - currentSpeed) * 0.5;

            _nextIntervalSpeed.Speed = nextSpeed;
            History.Add(_nextIntervalSpeed);
            _nextIntervalSpeed = new IntervalSpeed(_nextIntervalSpeed.Time.AddMilliseconds(Interval), 0);

            Current = nextSpeed;

            if (!_minInited || Min > Current)
            {
                _minInited = true;
                Min = Current;
            }

            if (Max < Current)
            {
                Max = Current;
            }

            _speedSum += Current;
            Avg = _speedSum / History.Count;
        }
    }
}