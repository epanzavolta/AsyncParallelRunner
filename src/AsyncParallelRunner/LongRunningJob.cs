using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class LongRunningJob
    {
        private readonly string _name;
        private readonly IJobTracer _logger;

        public LongRunningJob(string name, IJobTracer logger)
        {
            _name = name;
            _logger = logger;
        }

        public async Task ExecuteAsync(WorkType workType, TimeSpan duration)
        {
            _logger.Trace(_name, ActionType.Started);

            if (workType == WorkType.CpuBound)
                BusyWait(duration);
            else if (workType == WorkType.IOBound)
                await Task.Delay(duration);

            _logger.Trace(_name, ActionType.Stopped);
        }
        
        private void BusyWait(TimeSpan duration)
        {
            var startTime = DateTime.Now;

            // active spinning on the CPU
            while ((DateTime.Now - startTime) < duration)
            {
            }
        }
    }
}
