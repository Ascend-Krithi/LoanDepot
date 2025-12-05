using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LD_AutomationFramework.Config
{
    public class ConfigSettings
    {
        public static string Browser { get; set; }
        public static string Environment { get; set; }
        public static string AppUrl { get; set; }
        public static string CustomerPortalUrl { get; set; }
        public static bool ConfigSelectionRequired { get; set; }
        public static bool PaymentsDataDeletionRequired { get; set; }
        public static string TestReportFolder { get; set; }
        public static string CurrentReportFolder { get; set; }
        public static string ScreenshotFolder { get; set; }
        public static string LogFolderName { get; set; }
        public static string SingleReport { get; set; }
        public static string ReportName { get; set; }
        public static string UserProfileCheck { get; set; }
        public static int SmallWaitTime { get; set; }
        public static int WaitTime { get; set; }
        public static int MediumWaitTime { get; set; }
        public static int LongWaitTime { get; set; }
        public static int LoginWaitTime { get; set; }
        public static int NumberOfLoanTestDataRequired { get; set; }
        public static string JiraUrl { get; set; }
        public static string JiraUsername { get; set; }
        public static string JiraPassword { get; set; }
    }
}
