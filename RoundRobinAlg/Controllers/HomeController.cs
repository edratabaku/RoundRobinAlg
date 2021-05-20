﻿using RoundRobinAlg.Models;
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
                IsFinished = false,
                UpdatedBurstTime = burstTime
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
        /// Calculates the completion time, turn around time and waiting time for each process and displays the gantt chart for all processes.
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
                if (currentTimeSlice == currentStartTime + timeQuantum)
                {
                    result.Add(new ProcessesViewModel()
                    {
                        StartTime = currentStartTime,
                        EndTime = currentTimeSlice,
                        ProcessId = executingProcess.ProcessId
                    });
                    isCpuTaken = false;
                    processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().UpdatedBurstTime -= timeQuantum;
                    if (processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().UpdatedBurstTime == 0)
                    {
                        processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished = true;
                        processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().CompletionTime = currentTimeSlice;
                    }
                    currentStartTime = currentTimeSlice;
                    firstProcessInQueue = queue.FirstOrDefault();
                    CheckForArrivingProcesses(currentTimeSlice);
                    if (processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished == false)
                    {
                        queue.Add(processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault());
                    }
                    if (isCpuTaken == false)
                    {
                        if (firstProcessInQueue == null)
                        {
                            executingProcess = queue.FirstOrDefault();
                        }
                        else
                        {
                            executingProcess = firstProcessInQueue;
                        }
                        queue.Remove(queue.FirstOrDefault());
                        isCpuTaken = true;
                    }
                    currentTimeSlice++;
                }
                else if (executingProcess.UpdatedBurstTime < timeQuantum && executingProcess.UpdatedBurstTime == currentTimeSlice - currentStartTime)
                {
                    result.Add(new ProcessesViewModel()
                    {
                        StartTime = currentStartTime,
                        EndTime = currentTimeSlice,
                        ProcessId = executingProcess.ProcessId
                    });
                    isCpuTaken = false;
                    processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().IsFinished = true;
                    processes.Where(p => p.ProcessId == executingProcess.ProcessId).FirstOrDefault().CompletionTime = currentTimeSlice;
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
                    currentTimeSlice++;
                }
                else
                {
                    CheckForArrivingProcesses(currentTimeSlice);
                    currentTimeSlice++;
                }
            }
            CalculateTurnAroundTime(processes);
            CalculateWaitingTime(processes);
            ResultViewModel model = new ResultViewModel()
            {
                Processes = processes,
                GanttChartData = result,
                AverageTurnAroundTime = String.Format("{0:0.##}", CalculateAverageTurnAroundTime(processes)),
                AverageWaitingTime = String.Format("{0:0.##}", CalculateAverageWaitingTime(processes))
            };
            return View(model);
            //return RedirectToAction("ShowSolution", "Home", new { solution = model});
        }
        /// <summary>
        /// Checks if there are any arriving processes
        /// </summary>
        /// <param name="currentTime"></param>
        public void CheckForArrivingProcesses(int currentTime)
        {
            List<Process> arrivingProcesses = processes.Where(p => p.ArrivalTime == currentTime).ToList();
            foreach (var arrivingProcess in arrivingProcesses)
            {
                queue.Add(arrivingProcess);
            }
        }
        /// <summary>
        /// Calculates turnaround time for each process
        /// </summary>
        /// <param name="model"></param>
        public void CalculateTurnAroundTime(List<Process> model)
        {
            foreach (var process in model)
            {
                process.TurnAroundTime = process.CompletionTime - process.ArrivalTime.GetValueOrDefault();
            }
        }
        /// <summary>
        /// Calculates waiting time for each process
        /// </summary>
        /// <param name="model"></param>
        public void CalculateWaitingTime(List<Process> model)
        {
            foreach(var process in model)
            {
                process.WaitingTime = process.TurnAroundTime - process.BurstTime;
            }
        }
        /// <summary>
        /// Calculates average turn around time
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        public double CalculateAverageTurnAroundTime(List<Process> processes)
        {
            int total = 0;
            int numberOfProcesses = processes.Count();
            foreach(var process in processes)
            {
                total += process.TurnAroundTime;
            }
            return total / (numberOfProcesses * 1.0);
        }
        /// <summary>
        /// Calculates average waiting time
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        public double CalculateAverageWaitingTime(List<Process> processes)
        {
            int total = 0;
            int numberOfProcesses = processes.Count();
            foreach (var process in processes)
            {
                total += process.WaitingTime;
            }
            return total / (numberOfProcesses * 1.0);
        }
        /// <summary>
        /// Shows Gantt Chart and table of all processes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ShowSolution(ResultViewModel solution)
        {
            return View(solution);
        }
    }
}