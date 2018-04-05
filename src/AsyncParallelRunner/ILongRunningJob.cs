using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    internal interface ILongRunningJob
    {
        string Name { get; }

        Task ExecuteAsync(WorkType workType, TimeSpan duration);
    }
}