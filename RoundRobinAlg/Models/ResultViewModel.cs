using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoundRobinAlg.Models
{
    /// <summary>
    /// The result view model class
    /// </summary>
    public class ResultViewModel
    {
        /// <summary>
        /// List of processes
        /// </summary>
        public List<Process> Processes { get; set; }
        /// <summary>
        /// List of data to build Gantt Chart
        /// </summary>
        public List<ProcessesViewModel> GanttChartData { get; set; }
        /// <summary>
        /// The average turn around time
        /// </summary>
        public string AverageTurnAroundTime { get; set; }
        /// <summary>
        /// The average waiting time
        /// </summary>
        public string AverageWaitingTime { get; set; }
    }
}