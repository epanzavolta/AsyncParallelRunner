using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Async / Parallel demo.");

            var config = new RunConfiguration
            {
                JobNames = new[] { "A", "B", "C" },
                JobDuration = TimeSpan.FromSeconds(2),
                ExecutionMode = ReadExecutionMode(),
                WorkType = ReadWorkType()
            };

            Console.WriteLine($"\nSTARTING WORK ({config.WorkType}, {config.ExecutionMode})\n");

            await new Runner().RunAsync(config);

            Console.WriteLine("\nWORK COMPLETED\nPress any key to continue...");
            Console.ReadLine();
        }

        private static WorkType ReadWorkType()
        {
            Console.WriteLine("Choose work type (1:CPU bound,  2:I/O bound).");
            int workTypeNumber = Int32.Parse(Console.ReadLine());

            switch (workTypeNumber)
            {
                case 1:
                    return WorkType.CpuBound;
                case 2:
                    return WorkType.IOBound;
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }

        private static ExecutionMode ReadExecutionMode()
        {
            Console.WriteLine("Choose execution mode (1:Simple Async,  2:Task.Run).");
            int executionModeNumber = Int32.Parse(Console.ReadLine());

            switch (executionModeNumber)
            {
                case 1:
                    return ExecutionMode.SimpleAsync;
                case 2:
                    return ExecutionMode.TaskRun;
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }
    }
}
