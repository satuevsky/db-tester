using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTesterLib.Tester.Utils
{
    class SpeedLogger
    {
        /// <summary>
        /// Текущая скорость
        /// </summary>
        public double Current
        {
            get
            {
                if (!_started) return 0;
                return Approximate(_prevSpeed, CurrentIntervalSpeed().Speed);
            }
        }

        /// <summary>
        /// Средняя скорость
        /// </summary>
        public double Avg => (_speedSum + Current) / (History.Count + 1);

        /// <summary>
        /// Промежуток времени в милисекундах, по которому измеряется скорость
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// История скоростей
        /// </summary>
        public List<IntervalSpeed> History { get; private set; }

        private IntervalSpeed _currentIntervalSpeed;
        private double _speedSum;
        private double _prevSpeed;
        private bool _started;

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
            Start();
            CurrentIntervalSpeed().Speed += count;
        }

        public void Start()
        {
            if (!_started)
            {
                _started = true;
                _prevSpeed = 0;
                NewInterval();
            }
        }

        public void Stop()
        {
            if (_started)
            {
                _started = false;
                PushInterval();
            }
        }


        private void NewInterval()
        {
            NewInterval(DateTime.Now);
        }

        private void NewInterval(DateTime time)
        {
            _currentIntervalSpeed = new IntervalSpeed(time, 0);
        }

        private void PushInterval()
        {
            if (_currentIntervalSpeed == null) return;

            _prevSpeed = Approximate(_prevSpeed, _currentIntervalSpeed.Speed);
            _speedSum += _prevSpeed;
            History.Add(_currentIntervalSpeed);
            _currentIntervalSpeed = null;
        }

        private IntervalSpeed CurrentIntervalSpeed()
        {
            if (!_started) return null;

            DateTime
                time = _currentIntervalSpeed.Time.AddMilliseconds(Interval),
                now = DateTime.Now;

            while (time < now)
            {
                PushInterval();
                NewInterval(time);
                time = time.AddMilliseconds(Interval);
            }

            return _currentIntervalSpeed;
        }

        private double Approximate(double val1, double val2)
        {
            return val1 + (val2 - val1) * 0.8;
        }
    }
}