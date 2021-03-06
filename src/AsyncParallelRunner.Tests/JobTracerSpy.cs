﻿using System.Collections.Generic;

namespace AsyncParallelRunner.Tests
{
    struct TraceData
    {
        public string JobName { get; set; }

        public ActionType Action { get; set; }
    }

    class JobTracerSpy : IJobTracer
    {
        private readonly object _lockObj = new object();

        public List<TraceData> Data { get; } = new List<TraceData>();

        public void Trace(string jobName, ActionType action)
        {
            lock (_lockObj)
            {
                Data.Add(new TraceData
                {
                    JobName = jobName,
                    Action = action
                });
            }
        }
    }
}
