using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoundRobinAlg.Models
{
    /// <summary>
    /// Process view model class
    /// </summary>
    public class ProcessesViewModel
    {
        /// <summary>
        /// Time when process started execution is CPU
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// Time when execution stopped and process released CPU
        /// </summary>
        public int EndTime { get; set; }
        /// <summary>
        /// Id of process
        /// </summary>
        public string ProcessId { get; set; }
    }
}