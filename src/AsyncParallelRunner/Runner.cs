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
        {
        }

        public Runner(IJobTracer jobTracer)
        {
            _jobTracer = jobTracer;
        }

        public async Task RunAsync(RunConfiguration configuration)
        {
            var jobs = configuration.JobNames
                .Select(s => new LongRunningJob(s, _jobTracer))
                .ToList();
            
            var jobsTasks = jobs.Select(j => GetJobExecutionTask(j, configuration)).ToList();

            await Task.WhenAll(jobsTasks);
        }

        private Task GetJobExecutionTask(LongRunningJob job, RunConfiguration runConfiguration)
        {
            if (runConfiguration.ConcurrencyMode == ConcurrencyMode.Sequential)
                return job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration);

            return Task.Run(async () => await job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration));
        }
    }
}
