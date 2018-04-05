using System;
using System.Threading.Tasks;

namespace AsyncParallelRunner
{
    class LongRunningJobTracingDecorator : ILongRunningJob
    {
        private readonly ILongRunningJob _decoratee;
        private readonly IJobTracer _logger;

        public LongRunningJobTracingDecorator(ILongRunningJob decoratee, IJobTracer logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public string Name => _decoratee.Name;

        public async Task ExecuteAsync(WorkType workType, TimeSpan duration)
        {
            _logger.Trace(_decoratee.Name, ActionType.Started);

            await _decoratee.ExecuteAsync(workType, duration);

            _logger.Trace(_decoratee.Name, ActionType.Stopped);
        }
    }
}
