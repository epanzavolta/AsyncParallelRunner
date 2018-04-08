using System;
using System.Threading;

namespace AsyncParallelRunner
{
    public interface IJobTracer
    {
        void Trace(string jobName, ActionType action);
    }

    class JobTracer : IJobTracer
    {
        private readonly DateTime _startTime;
        private readonly object _lockObject = new object();

        public JobTracer(DateTime startTime)
        {
            _startTime = startTime;
        }
        
        public void Trace(string jobName, ActionType action)
        {
            lock (_lockObject)
            {
                var elapsed = DateTime.Now - _startTime;
                var elapsedSecondsText = Math.Round(elapsed.TotalSeconds, 3).ToString("0.000");

                Console.ResetColor();
                Console.Write($"After {elapsedSecondsText}s: \t");

                Console.ForegroundColor = (action == ActionType.Start) ? ConsoleColor.Green : ConsoleColor.Blue;
                Console.Write($"{action}");

                Console.ResetColor();
                Console.Write("\t job '");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{jobName}");

                Console.ResetColor();
                Console.WriteLine($"' on thread {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
