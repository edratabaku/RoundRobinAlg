using RoundRobinAlg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoundRobinAlg.Controllers
{
    /// <summary>
    /// Creates a new instance of the HomeController class
    /// </summary>
    public class HomeController : Controller
    {
        public static List<Process> processes = new List<Process>();
        public static int timeQuantum = 0;
        public static List<Process> queue = new List<Process>();
        /// <summary>
        /// Index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Action result to read data
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData()
        {
            return View();
        }
        /// <summary>
        /// Adds to processes list
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="burstTime"></param>
        /// <param name="arrivalTime"></param>
        /// <returns></returns>
        public JsonResult AddData(string processId, int burstTime, int? arrivalTime = 0)
        {
            processes.Add(new Process()
            {
                ProcessId = processId,
                BurstTime = burstTime,
                ArrivalTime = arrivalTime,
                IsFinished = false
            });
            return Json(new { status = "Success" });
        }
        /// <summary>
        /// Assigns value to time quantum variable and redirects to the ShowResult action method
        /// </summary>
        /// <param name="timeQuantumParam"></param>
        /// <returns></returns>
        public JsonResult AddTimeQuantum(int timeQuantumParam)
        {
            timeQuantum = timeQuantumParam;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ShowResult", "Home");
            return Json(new { Url = redirectUrl });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowResult()
        {
            List<ProcessesViewModel> result = new List<ProcessesViewModel>();
            int currentStartTime = 0;
            int currentTimeSlice = 0; 
            bool isCpuTaken = false;
            Process executingProcess = new Process();
            CheckForArrivingProcesses(currentTimeSlice);
            var firstProcessInQueue = queue.FirstOrDefault();
            if (isCpuTaken == false)
            {
                executingProcess = firstProcessInQueue;
                queue.Remove(queue.FirstOrDefault());
                isCpuTaken = true;
            }
            currentTimeSlice++;
            while (processes.Any(p => p.IsFinished == false))
            {
                if(currentTimeSlice == currentStartTime + timeQuantum)
                {
                    result.Add(new ProcessesViewModel()
                    {
                        StartTime = currentStartTime,
                        EndTime = currentTimeSlice,
                        ProcessId =executingProcess.ProcessId
                    });
                    isCpuTaken = false;
                    processes.Where(p=>p.ProcessId == executingProcess.ProcessId).FirstOrDefault().BurstTime -= timeQuantum;
                    if (processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().BurstTime == 0)
                    {
                        processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished = true;
                    }
                    currentStartTime = currentTimeSlice;
                    firstProcessInQueue = queue.FirstOrDefault();
                    CheckForArrivingProcesses(currentTimeSlice);
                    if(processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished == false)
                    {
                        queue.Add(processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault());
                    }
                    if (isCpuTaken == false)
                    {
                        executingProcess = firstProcessInQueue;
                        queue.Remove(queue.FirstOrDefault());
                        isCpuTaken = true;
                    }
                }
                else if(executingProcess.BurstTime < timeQuantum && executingProcess.BurstTime == currentTimeSlice - currentStartTime)
                {
                    isCpuTaken = false;
                    processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished = true;
                    currentStartTime = currentTimeSlice;
                    firstProcessInQueue = queue.FirstOrDefault();
                    CheckForArrivingProcesses(currentTimeSlice);
                    if (processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished == false)
                    {
                        queue.Add(processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault());
                    }
                    if (isCpuTaken == false)
                    {
                        executingProcess = firstProcessInQueue;
                        queue.Remove(queue.FirstOrDefault());
                        isCpuTaken = true;
                    }
                }
                else
                {
                    CheckForArrivingProcesses(currentTimeSlice);
                    currentTimeSlice++;
                }
            }
            return View(result);
        }
        public void CheckForArrivingProcesses(int currentTime)
        {
            List<Process> arrivingProcesses = processes.Where(p => p.ArrivalTime == currentTime).ToList();
            foreach (var arrivingProcess in arrivingProcesses)
            {
                queue.Add(arrivingProcess);
            }
        }

    }
}