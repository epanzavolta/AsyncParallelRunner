using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class Program
    {
        private const ConcurrencyMode Mode = ConcurrencyMode.Sequential;
        private const WorkType Type = WorkType.CpuBound;

        static async Task Main(string[] args)
        {
            var config = new RunConfiguration
            {
                JobNames = new[] { "A", "B", "C" },
                JobDuration = TimeSpan.FromSeconds(2),
                ConcurrencyMode = Mode,
                WorkType = Type
            };

            await new Runner().RunAsync(config);

            Console.WriteLine("COMPLETED");
            Console.ReadLine();
        }
    }
}
