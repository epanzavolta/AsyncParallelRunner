using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class JobTracingDecorator : IJob
    {
        private readonly IJob _decoratee;
        private readonly IJobTracer _logger;

        public JobTracingDecorator(IJob decoratee, IJobTracer logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public string Name => _decoratee.Name;

        public async Task ExecuteAsync(WorkType workType, TimeSpan duration)
        {
            _logger.Trace(_decoratee.Name, ActionType.Start);

            await _decoratee.ExecuteAsync(workType, duration);

            _logger.Trace(_decoratee.Name, ActionType.Finish);
        }
    }
}
