using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    public class Runner
    {
        private readonly IJobTracer _jobTracer;

        private readonly Func<IJob, RunConfiguration, Task> _executeJobFunc = 
            (job, config) => job.ExecuteAsync(config.WorkType, config.JobDuration);
        
        public async Task RunAsync(RunConfiguration configuration)
        {
            var jobTasks = configuration.JobNames
                .Select(CreateJob)
                .Select(job => GetJobExecutionTask(job, configuration));
            
            await Task.WhenAll(jobTasks);
        }

        private IJob CreateJob(string name)
        {
            return new JobTracingDecorator(
                new LongRunningJob(name), _jobTracer);
        }

        private Task GetJobExecutionTask(IJob job, RunConfiguration runConfiguration)
        {
            switch (runConfiguration.ExecutionMode)
            {
                case ExecutionMode.SimpleAsync:
                    return _executeJobFunc(job, runConfiguration);

                case ExecutionMode.TaskRun:
                    return Task.Run(async () => await _executeJobFunc(job, runConfiguration));

                default:
                    throw new ArgumentOutOfRangeException(nameof(runConfiguration.ExecutionMode));
            }
        }

        public Runner()
            : this(new JobTracer(DateTime.Now))
        { }

        public Runner(IJobTracer jobTracer)
        {
            _jobTracer = jobTracer;
        }
    }
}
