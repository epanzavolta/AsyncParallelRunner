using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class LongRunningJob : ILongRunningJob
    {
        public LongRunningJob(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public async Task ExecuteAsync(WorkType workType, TimeSpan duration)
        {
            if (workType == WorkType.CpuBound)
                SimulateCpuBoundWork(duration);
            else if (workType == WorkType.IOBound)
                await SimulateIOBoundWorkAsync(duration);
        }

        /// <summary>
        /// Simulates a CPU-intensive operation;
        /// in this case, just an active spinning on the CPU.
        /// </summary>
        /// <param name="duration"></param>
        private void SimulateCpuBoundWork(TimeSpan duration)
        {
            var startTime = DateTime.Now;
            while ((DateTime.Now - startTime) < duration)
            {
            }
        }

        /// <summary>
        /// Simulates an IO-bound operation (acess to files, database, ...)
        /// through a natively async interface;
        /// in this case, just a delay.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private static async Task SimulateIOBoundWorkAsync(TimeSpan duration)
        {
            await Task.Delay(duration);
        }

    }
}
