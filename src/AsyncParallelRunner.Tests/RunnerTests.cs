using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AsyncParallelRunner.Tests
{
    public class RunnerTests
    {
        [Fact]
        public async Task WhenCpuBound_AndAsync_ShouldExecuteJobsSequentiallyOneAfterEachOther()
        {
            var traceData = await RunAsync(ExecutionMode.Async, WorkType.CpuBound);

            var expectedTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Started},
                new TraceData{JobName = "A", Action = ActionType.Stopped},
                new TraceData{JobName = "B", Action = ActionType.Started},
                new TraceData{JobName = "B", Action = ActionType.Stopped},
                new TraceData{JobName = "C", Action = ActionType.Started},
                new TraceData{JobName = "C", Action = ActionType.Stopped}
            };

            traceData.Should().BeEquivalentTo(expectedTraceData);
        }

        [Fact]
        public async Task WhenCpuBound_AndParallel_ShouldStartAllJobsInParallelThenFinishAllJobs()
        {
            var traceData = await RunAsync(ExecutionMode.Parallel, WorkType.CpuBound);

            var expectedStartTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Started},
                new TraceData{JobName = "B", Action = ActionType.Started},
                new TraceData{JobName = "C", Action = ActionType.Started}
            };
            var expectedStopTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Stopped},
                new TraceData{JobName = "B", Action = ActionType.Stopped},
                new TraceData{JobName = "C", Action = ActionType.Stopped}
            };

            traceData.Count.Should().Be(6);

            traceData.Take(3).Should().BeEquivalentTo(expectedStartTraceData);
            traceData.Skip(3).Should().BeEquivalentTo(expectedStopTraceData);
        }

        [Fact]
        public async Task WhenIOBound_AndAsync_ShouldStartAllJobsInParallelThenFinishAllJobs()
        {
            var traceData = await RunAsync(ExecutionMode.Async, WorkType.IOBound);

            var expectedStartTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Started},
                new TraceData{JobName = "B", Action = ActionType.Started},
                new TraceData{JobName = "C", Action = ActionType.Started}
            };
            var expectedStopTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Stopped},
                new TraceData{JobName = "B", Action = ActionType.Stopped},
                new TraceData{JobName = "C", Action = ActionType.Stopped}
            };

            traceData.Count.Should().Be(6);

            traceData.Take(3).Should().BeEquivalentTo(expectedStartTraceData);
            traceData.Skip(3).Should().BeEquivalentTo(expectedStopTraceData);
        }

        [Fact]
        public async Task WhenIOBound_AndParallel_ShouldStartAllJobsInParallelThenFinishAllJobs()
        {
            var traceData = await RunAsync(ExecutionMode.Parallel, WorkType.IOBound);

            var expectedStartTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Started},
                new TraceData{JobName = "B", Action = ActionType.Started},
                new TraceData{JobName = "C", Action = ActionType.Started}
            };
            var expectedStopTraceData = new List<TraceData>
            {
                new TraceData{JobName = "A", Action = ActionType.Stopped},
                new TraceData{JobName = "B", Action = ActionType.Stopped},
                new TraceData{JobName = "C", Action = ActionType.Stopped}
            };

            traceData.Count.Should().Be(6);

            traceData.Take(3).Should().BeEquivalentTo(expectedStartTraceData);
            traceData.Skip(3).Should().BeEquivalentTo(expectedStopTraceData);
        }

        private async Task<List<TraceData>> RunAsync(ExecutionMode executionMode, WorkType workType)
        {
            var config = new RunConfiguration
            {
                JobNames = new[] { "A", "B", "C" },
                JobDuration = TimeSpan.FromMilliseconds(500),
                ExecutionMode = executionMode,
                WorkType = workType
            };

            var tracer = new JobTracerSpy();
            var sut = new Runner(tracer);

            await sut.RunAsync(config);

            return tracer.Data;
        }
    }
}
