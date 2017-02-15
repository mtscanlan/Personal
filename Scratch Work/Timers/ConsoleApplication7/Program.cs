using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var timespan = new CustomTimerOne();
                var x = timespan.RealTime();
                Thread.Sleep(1000);
                var y = timespan.RealTime();
                sw.Stop();
                var z = new DateTime(y-x).TimeOfDay;
                Console.WriteLine($"Stopwatch {sw.ElapsedTicks}/{sw.ElapsedMilliseconds} - CustomTimerOne {z.Ticks}/{z.TotalMilliseconds}");

                sw.Reset();

                sw.Start();
                var x1 = new CustomTimerTwo();
                x1.Start();
                Thread.Sleep(1000);
                x1.End();
                sw.Stop();
                var z1 = new DateTime(x1.GetDuration().Ticks).TimeOfDay;

                Console.WriteLine($"Stopwatch {sw.ElapsedTicks}/{sw.ElapsedMilliseconds} - CustomTimerTwo {z1.Ticks}/{z1.TotalMilliseconds}");
            }
        }
    }
    public class CustomTimerOne
    {
        private long globalTicks = DateTime.UtcNow.Ticks;                               // low-precision system date time
        private long localTicks = Stopwatch.GetTimestamp();                             // high-precision local clock measurement
        private int globalFrequency = 10000 * 1000;                                     // .NET constant (10000 ticks per millisecond and 1000 milliseconds per second): http://msdn.microsoft.com/en-us/library/system.datetime.ticks.aspx
        private long localFrequency = Stopwatch.Frequency;                              // runtime constant: http://msdn.microsoft.com/en-us/library/system.diagnostics.stopwatch.frequency.aspx


        private long Current { get; }

        public long RealTime()
        {
            // capture the high precision time
            var timestamp = Stopwatch.GetTimestamp();
            // determine how much has passed since our high-precision baseline
            var delta = timestamp - localTicks;
            // map the high precision delta from the local clock space into the global clock space:
            var globalDelta = (delta * globalFrequency) / localFrequency;
            // high-precision system time
            var ticks = globalTicks + globalDelta;

            return ticks;
        }
    }

    public class CustomTimerTwo
    {
        private Stopwatch _dbStopwatch;
        
        public TimeSpan GetDuration() 
        {
            return _dbStopwatch.Elapsed;
        }

        public CustomTimerTwo()
        {
            _dbStopwatch = new Stopwatch(); 
        }
        
        public void Start()
        {
            _dbStopwatch = Stopwatch.StartNew();
        }
        
        public void End()
        {
            _dbStopwatch.Stop();
        }
    }
}
