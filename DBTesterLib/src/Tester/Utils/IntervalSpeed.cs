using System;

namespace DBTesterLib.Tester.Utils
{
    class IntervalSpeed
    {
        public DateTime Time { get; private set; }

        public double Speed { get; set; }

        public IntervalSpeed(DateTime time, double speed)
        {
            Time = time;
            Speed = speed;
        }
    }
}