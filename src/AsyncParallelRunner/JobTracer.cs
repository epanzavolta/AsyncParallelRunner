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

        public JobTracer(DateTime startTime)
        {
            _startTime = startTime;
        }
        
        public void Trace(string jobName, ActionType action)
        {
            var elapsed = DateTime.Now - _startTime;
            var elapsedSecondsText = Math.Round(elapsed.TotalSeconds, 3).ToString("#.###");
            Console.WriteLine($"After {elapsedSecondsText}s: \t{action} job '{jobName}' on thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
