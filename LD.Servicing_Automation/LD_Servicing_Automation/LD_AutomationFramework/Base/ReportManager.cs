using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework.Config;
using log4net;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;

namespace LD_AutomationFramework.Base
{
    public class ReportManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int instanceCount = 0;

        private static ExtentReports _extentReportsInstance;
        public static MethodBase testCallerMethod { get; set; }
        public static ExtentReports GetExtentReportsInstance
        {
            get
            {
                if(ConfigSettings.SingleReport != "True")
                {
                    instanceCount = 0;
                }
                if(instanceCount == 0)
                {
                    instanceCount += 1;
                    new ReportManager().WaitForReportInstace().Wait();
                }
                else
                {
                    int timeOutInMillis = 7000;
                    DateTime executionStartTime = DateTime.Now;
                    TimeSpan timeDiff = (DateTime.Now - executionStartTime);
                    while(((int)timeDiff.TotalMilliseconds) < timeOutInMillis && _extentReportsInstance == null)
                    {
                        Thread.Sleep(100);
                        timeDiff = (DateTime.Now - executionStartTime);
                    }
                }
                return _extentReportsInstance;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        ///<summary>
        ///Method for the creation of ExtentReport instance
        ///</summary>
        ///<param name="className">Pass test class name with which report file will be created.</param>
        ///<returns>Returns instance of Extent Reports.</returns>
        public ExtentReports Instance(string testClassName)
        {
            string browserName = string.Empty, appPath = string.Empty, reportFolderPath = string.Empty, reportName = string.Empty;
            ExtentReports extent;
            ExtentSparkReporter htmlReporter;

            try
            {
                browserName = ConfigSettings.Browser;
                log.Info("Report creation started from ReportManager Instance: " + browserName);
                appPath = UtilAdditions.GetRootDirectory();
                ConfigSettings.CurrentReportFolder = System.DateTime.Now.ToString("MM_dd_yyyy");
                reportFolderPath = Path.Combine(appPath, ConfigSettings.TestReportFolder, ConfigSettings.CurrentReportFolder);
                UtilAdditions.CreateDirectory(reportFolderPath);
                //UtilAdditions.CreateDirectory(Path.Combine(reportFolderPath, ConfigSettings.ScreenshotFolder)); Code kept for reference in case screenshot needs to be saved separately.
                ConfigSettings.ReportName = testClassName + DateTime.Now.ToString("_dd_MMMM_hh_mm_ss_tt_") + browserName + ".html";
                htmlReporter = new ExtentSparkReporter(Path.Combine(reportFolderPath, ConfigSettings.ReportName));
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
            }
            catch (Exception ex)
            {
                log.Error("Failed while creating session of Reporting Instance: " + ex.Message);
                log.Error(ex.StackTrace);
                throw;
            }
            return extent;
        }

        ///<summary>
        ///Method to initialize Extent Report instance. This method will create a single instance of extent report.
        ///</summary>
        ///<param name="method">Pass MethodBase reference of Test Method</param>
        private async Task<ExtentReports> InitializeExtentReport(MethodBase method)
        {
            ExtentReports extentReports = null;
            await Task.Run(() =>
            {
                if (_extentReportsInstance == null)
                {
                    List<string> categories = new List<string>();
                    foreach (var attribute in (IEnumerable<TestCategoryAttribute>)method
                            .GetCustomAttributes(typeof(TestCategoryAttribute), true))
                    {
                        foreach (var category in attribute.TestCategories)
                        {
                            categories.Add(category);
                        }
                    }
                    if (ConfigSettings.SingleReport == "True")
                    {
                        extentReports = Instance(categories[0]);
                    }
                    else
                    {
                        extentReports = Instance(method.ReflectedType.Name);
                    }
                }
                else
                {
                    extentReports = _extentReportsInstance;
                }
            });
            return extentReports;
        }

        public async Task WaitForReportInstace()
        {
            ExtentReports[] reports = null;
            if(ConfigSettings.SingleReport != "True")
            {
                _extentReportsInstance = null;
            }
            List<Task<ExtentReports>> reportCreationTasks = new List<Task<ExtentReports>>();
            reportCreationTasks.Add(InitializeExtentReport(testCallerMethod));
            reports = await System.Threading.Tasks.Task.WhenAll(reportCreationTasks);
            if(reports != null)
            {
                _extentReportsInstance = reports[0];
            }
        }
    }
}
