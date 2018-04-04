using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class Program
    {
        private const ExecutionMode Mode = ExecutionMode.Async;
        private const WorkType Type = WorkType.CpuBound;

        static async Task Main(string[] args)
        {
            var config = new RunConfiguration
            {
                JobNames = new[] { "A", "B", "C" },
                JobDuration = TimeSpan.FromSeconds(2),
                ExecutionMode = Mode,
                WorkType = Type
            };

            await new Runner().RunAsync(config);

            Console.WriteLine("COMPLETED");
            Console.ReadLine();
        }
    }
}
