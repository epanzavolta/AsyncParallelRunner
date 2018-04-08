using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    internal interface IJob
    {
        string Name { get; }

        Task ExecuteAsync(WorkType workType, TimeSpan duration);
    }
}