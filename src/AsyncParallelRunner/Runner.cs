using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    public class Runner
    {
        private readonly IJobTracer _jobTracer;

        public Runner()
        : this(new JobTracer(DateTime.Now))
        { }

        public Runner(IJobTracer jobTracer)
        {
            _jobTracer = jobTracer;
        }

        public async Task RunAsync(RunConfiguration configuration)
        {
            var jobs = configuration.JobNames
                .Select(s => new LongRunningJob(s, _jobTracer))
                .ToList();
            
            await Task.WhenAll(jobs.Select(j => GetJobExecutionTask(j, configuration)));
        }

        private Task GetJobExecutionTask(LongRunningJob job, RunConfiguration runConfiguration)
        {
            switch (runConfiguration.ExecutionMode)
            {
                case ExecutionMode.Async:
                    return job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration);

                case ExecutionMode.Parallel:
                    return Task.Run(async () => await job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration));

                default:
                    throw new ArgumentOutOfRangeException(nameof(runConfiguration.ExecutionMode));
            }
        }
    }
}
