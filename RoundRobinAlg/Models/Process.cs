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
        /// The process identifier
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
        /// Updated burst time for process
        /// </summary>
        public int UpdatedBurstTime { get; set; }
        /// <summary>
        /// If process has been fully executed
        /// </summary>
        public bool IsFinished { get; set; }
        /// <summary>
        /// Completion Time for Process
        /// </summary>
        public int CompletionTime { get; set; }
        /// <summary>
        /// Turn Around time for process
        /// </summary>
        public int TurnAroundTime { get; set; }
        /// <summary>
        /// Waiting Time For Process
        /// </summary>
        public int WaitingTime { get; set; }
    }
}