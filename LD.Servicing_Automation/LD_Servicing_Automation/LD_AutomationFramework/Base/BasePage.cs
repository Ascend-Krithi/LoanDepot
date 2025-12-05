using System;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LD_AutomationFramework.Config;
using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;
using log4net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Collections;
using iTextSharp.text;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LD_AutomationFramework.Pages;
using AventStack.ExtentReports.Model;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using WebDriverManager;
using WebDriverManager.Helpers;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace LD_AutomationFramework
{
    [TestClass]
    public class BasePage
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IWebDriver _driver { get; set; }
        public ExtentReports extent { get; set; }
        public ExtentTest test { get; set; }
        public TestContext testContext { get; set; }        
        public XmlDocument xmlDocument { get; set; }

        public string username, password;
        
        string testCaseDescription, testcaseName;

        int browserProcessID = 0;

        public static int executionCount = 0;

        public static Hashtable credUsageTable = new Hashtable();
        
        public static Hashtable executedTestCategories = new Hashtable();

        public static Hashtable executedTests = new Hashtable();

        public static Hashtable executedTestStatus = new Hashtable();        

        /// <summary>
        /// Method to initialize the framework prerequisites required for test script execution
        /// </summary>
        /// <param name="tc">test context</param>
        /// <param name="isApi">true/false</param>
        public void InitializeFramework(TestContext tc, bool isApi = false)
        {
            testContext = tc;

            //Report Initialization
            ReportManager.testCallerMethod = GetType().GetMethod(testContext.TestName);
            extent = ReportManager.GetExtentReportsInstance;
            UtilAdditions.GetTestCaseDetails(testContext, GetType().GetMethod(testContext.TestName), out testCaseDescription, out testcaseName);
            log.Info($"****************************************** Test Execution '{testcaseName}' started ****************************");

            List<string> categories = new List<string>();
            foreach (var attribute in (IEnumerable<TestCategoryAttribute>)ReportManager.testCallerMethod
                    .GetCustomAttributes(typeof(TestCategoryAttribute), true))
            {
                foreach (var category in attribute.TestCategories)
                {
                    categories.Add(category);
                }
            }
            if (categories.Count == 1)
                test = extent.CreateTest(testcaseName, testCaseDescription).AssignCategory(categories[0]);
            else
                test = extent.CreateTest(testcaseName, testCaseDescription).AssignCategory(categories[1]);

            //Credential selection based on execution type - Sequential or Parallel
            string isParallelExecution = Convert.ToString(testContext.Properties["DisableParallelization"]);
            if (!string.IsNullOrEmpty(isParallelExecution) && isParallelExecution.ToLower().Equals("false"))
                InitializeCredentialsForParallelExecution();
            else
                InitializeTestCredentialsFromXml();

            //Decryption            
            password = new EncryptionManager(test).DecryptDataWithAes(password);

            if (!isApi)
            {
                //Browser Launch
                test.Log(Status.Info, $"Browser selected for test execution: '{ConfigSettings.Browser}'.");
                
                if (ConfigSettings.Browser.Equals("Chrome"))
                    new DriverManagerUtility().DownloadChromeDriver(VersionResolveStrategy.MatchingBrowser, Architecture.X32);
                else if (ConfigSettings.Browser.Equals("Edge"))
                    new DriverManagerUtility().DownloadEdgeDriver(VersionResolveStrategy.MatchingBrowser, Architecture.X32);

                _driver = new Browser().WebDriver(ConfigSettings.Browser, out browserProcessID);
                test.Log(Status.Info, "Launched the browser.");
                _driver.Manage().Cookies.DeleteAllCookies();                

                //URL Launch
                test.Log(Status.Info, "\"*********************** Test execution started ***********************\"");
                _driver.Navigate().GoToUrl(ConfigSettings.AppUrl);
                test.Log(Status.Info, $"Navigated to the URL: '{ConfigSettings.AppUrl}'.");
            }
        }

        /// <summary>
        /// Method to initialize test data credentials for sequential execution
        /// </summary>
        public void InitializeTestCredentialsFromXml()
        {
            bool flag = false;
            try
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(Path.Combine(UtilAdditions.GetRootDirectory(), "TestData/TestCredentials.xml"));
                string[] testCases = testcaseName.Split('.');

                foreach(XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    if (node.Name.Equals(testCases[testCases.Length - 2]))
                    {
                        foreach(XmlNode childNode in node.ChildNodes)
                        {
                            if(childNode.Name.Equals(testCases[testCases.Length - 1]))
                            {
                                username = childNode.ChildNodes[0].InnerText;
                                password = childNode.ChildNodes[1].InnerText;
                                flag = true;
                            }
                            if (flag)
                                break;
                        }
                    }
                    if (flag)
                        break;
                }                
                    
            }
            catch(Exception ex)
            {
                log.Error($"Exception occurred while retrieving test data credentials from xml file: {ex.Message}");
            }
            _driver.ReportResult(test, flag, "Successfully retrieved test data credentials from xml file", "Failed while retrieving test data credentials from xml file");
        }

        /// <summary>
        /// Method to initialize test data credentials for parallel execution
        /// </summary>
        public void InitializeCredentialsForParallelExecution()
        {
            int getCredTimeOutIMillis = (int)TimeSpan.FromSeconds(ConfigSettings.WaitTime).TotalMilliseconds;
            bool userFound = false; 
            DateTime executionStartTime = DateTime.Now;
            TimeSpan timeDiff = (DateTime.Now - executionStartTime);
            try
            {                
                if (credUsageTable.Count == 0)
                {
                    string allUserCredentials = UtilAdditions.GetExtractedXmlData("TestCredentials.xml", "xml/ParallelExecution_Credentials/Credentials");
                    string[] userCredentialsArray = allUserCredentials.Split('|');
                    foreach (string credentials in userCredentialsArray)
                    {
                        if (!string.IsNullOrEmpty(credentials.Trim()) && !credUsageTable.ContainsKey(credentials.Trim()))
                            credUsageTable.Add(credentials.Trim(), "not_in_use");
                    }
                }                 

                while (((int)timeDiff.TotalMilliseconds) < getCredTimeOutIMillis && !userFound)
                {
                    List<string> keys = credUsageTable.Keys.Cast<string>().ToList();
                    foreach (string cred in keys)
                    {
                        if (credUsageTable[cred].ToString().ToLower().Equals("not_in_use"))
                        {
                            credUsageTable[cred] = "in_use_now";
                            username = cred.Split(';')[0];
                            password = cred.Split(';')[1];
                            userFound = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred while initializing credentials for parallel execution: " + ex.Message);
            }
            timeDiff = (DateTime.Now - executionStartTime);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            try
            {   
                if (_driver != null)
                {
                    CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
                    commonServices.LogoutOfTheApplication();
                    _driver.Close();
                    _driver.Quit();
                    _driver?.Dispose();
                    test.Log(Status.Info, "Closed all browser sessions.");
                }                
                if (test.Status != Status.Pass && test.Status != Status.Warning)
                    Assert.Fail();
                test.Log(Status.Info, "\"*********************** Test execution completed ***********************\"");
            }
            catch (UnhandledAlertException)
            {
                if(_driver != null)
                {
                    var iAlert = _driver.SwitchTo().Alert();
                    if(iAlert == null)
                        throw new ArgumentNullException(nameof(iAlert));
                    Console.WriteLine(iAlert.Text);
                    iAlert.Accept();
                }
            }
            catch(Exception ex)
            {
                Assert.Fail();
            }
            finally
            {
                #region CreatingExecutionSummaryForEmail

                int categoryCount = 1, testCaseCount = 0;
                string category = string.Empty, testCaseCode = string.Empty;
                foreach (var attribute in (IEnumerable<TestCategoryAttribute>)ReportManager.testCallerMethod
                            .GetCustomAttributes(typeof(TestCategoryAttribute), true))
                {
                    foreach (var value in attribute.TestCategories)
                    {
                        if (!string.IsNullOrEmpty(category))
                            category += " | ";
                        category += value;
                    }
                }
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.AgentPortal))
                    testCaseCode = "TC";
                else if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                    testCaseCode = "TPR";
                else if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.PaymentEngine))
                    testCaseCode = "PE";
                if (ReportManager.testCallerMethod.Name.Contains(testCaseCode))
                {                    
                    string methodName = ReportManager.testCallerMethod.Name;
                    int substring1 = methodName.IndexOf(testCaseCode + "_");
                    int substring2 = methodName.IndexOf("_" + testCaseCode);

                    methodName = methodName.Substring(substring1, substring2);
                    string[] testCasesAssociatedToTestScript = methodName.Split('_');
                    testCaseCount = testCasesAssociatedToTestScript.Length - 1;
                }
                else
                    testCaseCount = 1;

                if (!executedTestCategories.Contains(category))
                    executedTestCategories.Add(category, testCaseCount);
                else
                {
                    categoryCount = Convert.ToInt32(executedTestCategories[category]);
                    categoryCount = categoryCount + testCaseCount;
                    executedTestCategories[category] = categoryCount;
                }
                
                executedTests.Add(ReportManager.testCallerMethod.Name, test.Status);
                
                UtilAdditions.WriteTestSummaryDetailsToXmlFile(executedTests, executedTestCategories);

                #endregion CreatingExecutionSummaryForEmail

                extent.Flush();
                password = new EncryptionManager(test).EncryptDataWithAes(password);
                credUsageTable[username + ";" + password] = "not_in_use";
                if(browserProcessID != 0)
                {
                    try
                    {
                        Process process = Process.GetProcessById(browserProcessID);
                        process.Kill();
                    }
                    catch (InvalidOperationException) { }
                    catch (ArgumentException) { }
                }
            }
        }
    }    
}
