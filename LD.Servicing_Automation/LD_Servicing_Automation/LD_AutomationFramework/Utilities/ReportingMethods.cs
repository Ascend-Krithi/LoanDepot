using System;
using log4net;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using System.Data;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Data.OleDb;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using AventStack.ExtentReports.Model;
using AutoIt;
using OpenQA.Selenium.Interactions;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Threading;

namespace LD_AutomationFramework.Utilities
{
    public static class ReportingMethods
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Method to add Log to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="message">The Log message</param>
        public static void Log(ExtentTest extentTest, string message)
        {

            extentTest.Log(Status.Info, message);
        }

        /// <summary>
        /// Method to verify expected and actual are equal and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="expected">Expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionEqual(ExtentTest extentTest, string expected, string actual, string message)
        {
            if (actual.Equals(expected))
            {
                extentTest.Log(Status.Pass, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");

            }
            else
            {
                extentTest.Log(Status.Fail, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");
            }
        }

        /// <summary>
        /// Method to verify actual list contains expected list and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="expectedList">Expected List</param>
        /// <param name="actualList">Actual List</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionListContains(ExtentTest extentTest, List<string> expectedList, List<string> actualList, string message)
        {
            if (expectedList.All(item => actualList.Contains(item)))
            {
                extentTest.Log(Status.Pass, message + " | Expected value - " + @"<b>" + string.Join(", ", expectedList) + @"</b>" + " Actual value - " + @"<b>" + string.Join(", ", actualList) + @"</b>");

            }
            else
            {
                extentTest.Log(Status.Fail, message + " | Expected value - " + @"<b>" + string.Join(", ", expectedList) + @"</b>" + " Actual value - " + @"<b>" + string.Join(", ", actualList) + @"</b>");
            }
        }

        /// <summary>
        /// Method to verify expected and actual are equal with ignoring case and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="expected">Expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionEqualIgnoreCase(ExtentTest extentTest, string expected, string actual, string message)
        {

            if (actual.Equals(expected, StringComparison.InvariantCultureIgnoreCase))
            {
                extentTest.Log(Status.Pass, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");

            }
            else
            {
                extentTest.Log(Status.Fail, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");
            }
        }

        /// <summary>
        /// Method to verify actual contains expected and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="expected">Expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionContains(ExtentTest extentTest, string expected, string actual, string message)
        {

            if (actual.Contains(expected))
            {
                extentTest.Log(Status.Pass, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");

            }
            else
            {
                extentTest.Log(Status.Fail, message + " | Expected value - " + @"<b>" + expected + @"</b>" + " Actual value - " + @"<b>" + actual + @"</b>");
            }
        }

        /// <summary>
        /// Method to verify assertionResult is True and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="assertionResult">True/False</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionTrue(ExtentTest extentTest, bool assertionResult, string message)
        {
            if (assertionResult)
            {
                extentTest.Log(Status.Pass, message);
            }
            else
            {
                extentTest.Log(Status.Fail, message);
            }
        }

        /// <summary>
        /// Method to verify assertionResult is False and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="assertionResult">True/False</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionFalse(ExtentTest extentTest, bool assertionResult, string message)
        {
            if (!assertionResult)
            {
                extentTest.Log(Status.Pass, message);
            }
            else
            {
                extentTest.Log(Status.Fail, message);
            }
        }

        /// <summary>
        /// Method to verify actual list Equals expected list and add result to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="expectedList">Expected List</param>
        /// <param name="actualList">Actual List</param>
        /// <param name="message">The Verification message</param>
        public static void LogAssertionListEqual(ExtentTest extentTest, List<string> expectedList, List<string> actualList, string message)
        {
            bool flag = true;
            if (actualList.Count == expectedList.Count)
            {
                foreach (string actualvalue in actualList)
                {
                    if (!expectedList.Contains(actualvalue))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                flag = false;
            }

            if (flag)
            {
                extentTest.Log(Status.Pass, message + " | Expected value - " + @"<b>" + string.Join(", ", expectedList) + @"</b>" + " Actual value - " + @"<b>" + string.Join(", ", actualList) + @"</b>");

            }
            else
            {
                extentTest.Log(Status.Fail, message + " | Expected value - " + @"<b>" + string.Join(", ", expectedList) + @"</b>" + " Actual value - " + @"<b>" + string.Join(", ", actualList) + @"</b>");
            }
        }

    }
}
