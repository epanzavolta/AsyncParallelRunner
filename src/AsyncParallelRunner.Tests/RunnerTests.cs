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
        public async Task WhenCpuBound_AndSequential_ShouldExecuteJobsOneAfterEachOther()
        {
            var traceData = await RunAsync(ConcurrencyMode.Sequential, WorkType.CpuBound);

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
        public async Task WhenCpuBound_AndParallel_ShouldStartAllJobsThenFinishAllJobs()
        {
            var traceData = await RunAsync(ConcurrencyMode.Parallel, WorkType.CpuBound);

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
        public async Task WhenIOBound_AndSequential_ShouldStartAllJobsThenFinishAllJobs()
        {
            var traceData = await RunAsync(ConcurrencyMode.Sequential, WorkType.IOBound);

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
        public async Task WhenIOBound_AndParallel_ShouldStartAllJobsThenFinishAllJobs()
        {
            var traceData = await RunAsync(ConcurrencyMode.Parallel, WorkType.IOBound);

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

        private async Task<List<TraceData>> RunAsync(ConcurrencyMode concurrencyMode, WorkType workType)
        {
            var config = new RunConfiguration
            {
                JobNames = new[] { "A", "B", "C" },
                JobDuration = TimeSpan.FromSeconds(1),
                ConcurrencyMode = concurrencyMode,
                WorkType = workType
            };

            var tracer = new JobTracerSpy();
            var sut = new Runner(tracer);

            await sut.RunAsync(config);

            return tracer.Data;
        }
    }
}
