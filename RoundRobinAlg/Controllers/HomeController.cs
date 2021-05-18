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
        public static List<Process> Processes = new List<Process>();
        public static int timeQuantum = 0;
        public static List<Process> Queue = new List<Process>();
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
            Processes.Add(new Process()
            {
                ProcessId = processId,
                BurstTime = burstTime,
                ArrivalTime = arrivalTime
            });
            return Json(new { status = "Success" });
        }
        public JsonResult AddTimeQuantum(int timeQuantumParam)
        {
            timeQuantum = timeQuantumParam;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ShowResult", "Home");
            return Json(new { Url = redirectUrl });
        }
    }
}