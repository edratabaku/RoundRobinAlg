using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoundRobinAlg.Models
{
    /// <summary>
    /// The process class
    /// </summary>
    public class Process
    {
        /// <summary>
        /// The process id
        /// </summary>
        public string ProcessId { get; set; }
        /// <summary>
        /// Nullable arrival time for process
        /// </summary>
        public int? ArrivalTime { get; set; }
        /// <summary>
        /// Burst time for process
        /// </summary>
        public int BurstTime { get; set; }
        /// <summary>
        /// If process has been fully executed
        /// </summary>
        public bool IsFinished { get; set; }
    }
}