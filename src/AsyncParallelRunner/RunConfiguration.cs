using System;
using System.Collections.Generic;

namespace AsyncParallelRunner
{
    public class RunConfiguration
    {
        public IEnumerable<string> JobNames { get; set; }

        public ConcurrencyMode ConcurrencyMode { get; set; }

        public WorkType WorkType { get; set; }

        public TimeSpan JobDuration { get; set; }
    }
}
