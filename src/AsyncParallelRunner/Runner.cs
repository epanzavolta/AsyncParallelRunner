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
            var jobTasks = configuration.JobNames
                .Select(CreateJob)
                .Select(job => GetJobExecutionTask(job, configuration))
                .ToList();
            
            await Task.WhenAll(jobTasks);
        }

        private ILongRunningJob CreateJob(string name)
        {
            return new LongRunningJobTracingDecorator(
                new LongRunningJob(name), _jobTracer);
        }

        private Task GetJobExecutionTask(ILongRunningJob job, RunConfiguration runConfiguration)
        {
            switch (runConfiguration.ExecutionMode)
            {
                case ExecutionMode.SimpleAsync:
                    return job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration);

                case ExecutionMode.TaskRun:
                    return Task.Run(async () => await job.ExecuteAsync(runConfiguration.WorkType, runConfiguration.JobDuration));

                default:
                    throw new ArgumentOutOfRangeException(nameof(runConfiguration.ExecutionMode));
            }
        }
    }
}
