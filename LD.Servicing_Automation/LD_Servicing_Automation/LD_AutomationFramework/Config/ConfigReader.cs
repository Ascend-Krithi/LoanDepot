using LD_AutomationFramework.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Web;
using System.Windows.Forms;
using System.Xml.XPath;

namespace LD_AutomationFramework.Config
{
    public class ConfigReader
    {
        /// <summary>
        /// Method to map the config file values to static variables
        /// </summary>
        public static void ConfigValueMappings()
        {
            var configFileName = "";
            string configBatchFile = UtilAdditions.GetRootDirectory();

            if (configBatchFile.Contains(Constants.SolutionProjectNames.AgentPortal))
            {
                configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\APConfig.xml";
            }
            else if (configBatchFile.Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\CPConfig.xml";
            }
            else if (configBatchFile.Contains(Constants.SolutionProjectNames.PaymentEngine))
            {
                configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\PEConfig.xml";
            }
            else
            {
                configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\MSPConfig.xml";
            }
            using (var stream = new FileStream(configFileName, FileMode.Open))
            {

                XPathDocument document = new XPathDocument(stream);
                XPathNavigator navigator = document.CreateNavigator();

                //Map ConfigSelectionRequired variable
                XPathItem configSelectionRequired = navigator.SelectSingleNode("Portal/RunSettings/ConfigSelectionRequired");
                ConfigSettings.ConfigSelectionRequired = configSelectionRequired.ValueAsBoolean;
            }

            if (ConfigSettings.ConfigSelectionRequired)
            {
                configBatchFile += @"\Config\ConfigSelector.bat";
                Process.Start(configBatchFile).WaitForExit();

                if (configBatchFile.Contains(Constants.SolutionProjectNames.AgentPortal))
                {
                    configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\APConfig.xml";
                }
                else if (configBatchFile.Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\CPConfig.xml";
                }
                else
                {
                    configFileName = Environment.CurrentDirectory.ToString() + "\\Config\\MSPConfig.xml";
                }
            }
            using (var stream = new FileStream(configFileName, FileMode.Open))
            {
                #region CommonSettings

                XPathDocument document = new XPathDocument(stream);
                XPathNavigator navigator = document.CreateNavigator();

                //Map Browser type
                XPathItem browser = navigator.SelectSingleNode("Portal/RunSettings/Browser");
                ConfigSettings.Browser = browser.Value;

                //Map Application environment
                XPathItem environment = navigator.SelectSingleNode("Portal/RunSettings/Environment");
                ConfigSettings.Environment = environment.Value;

                string nodeName = null;
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.MSP))
                    nodeName = "Portal/RunSettings/AppUrl";
                else
                {
                    if (ConfigSettings.Environment == "QA")
                        nodeName = "Portal/RunSettings/QAAppUrl";
                    else if (ConfigSettings.Environment == "SG")
                        nodeName = "Portal/RunSettings/SGAppUrl";
                    else if (ConfigSettings.Environment == "PROD")
                        nodeName = "Portal/RunSettings/PRODAppUrl";
                }

                //Map Application Url
                XPathItem appUrl = navigator.SelectSingleNode(nodeName);
                ConfigSettings.AppUrl = appUrl.Value;

                //Map PaymentsDataDeletionRequired variable
                XPathItem paymentsDataDeletionRequired = navigator.SelectSingleNode("Portal/RunSettings/PaymentsDataDeletionRequired");
                ConfigSettings.PaymentsDataDeletionRequired = paymentsDataDeletionRequired.ValueAsBoolean;


                //Test report folder name
                XPathItem testReportFolder = navigator.SelectSingleNode("Portal/RunSettings/TestReportFolder");
                ConfigSettings.TestReportFolder = testReportFolder.Value;

                //Screenshot folder name
                XPathItem screenshotFolder = navigator.SelectSingleNode("Portal/RunSettings/ScreenshotFolder");
                ConfigSettings.ScreenshotFolder = screenshotFolder.Value;

                //Create a single extent report or not
                XPathItem singleReport = navigator.SelectSingleNode("Portal/RunSettings/SingleReport");
                ConfigSettings.SingleReport = singleReport.Value;

                //User Profile Check 
                XPathItem userProfileCheck = navigator.SelectSingleNode("Portal/RunSettings/UserProfileCheck");
                ConfigSettings.UserProfileCheck = userProfileCheck.Value;

                //Small Wait time
                XPathItem smallWaitTime = navigator.SelectSingleNode("Portal/RunSettings/smallWaitTime");
                ConfigSettings.SmallWaitTime = Convert.ToInt32(smallWaitTime.Value);

                //Wait time
                XPathItem waitTime = navigator.SelectSingleNode("Portal/RunSettings/waitTime");
                ConfigSettings.WaitTime = Convert.ToInt32(waitTime.Value);

                //Medium Wait time
                XPathItem mediumWaitTime = navigator.SelectSingleNode("Portal/RunSettings/mediumWait");
                ConfigSettings.MediumWaitTime = Convert.ToInt32(mediumWaitTime.Value);

                //Long Wait time
                XPathItem longWaitTime = navigator.SelectSingleNode("Portal/RunSettings/longWait");
                ConfigSettings.LongWaitTime = Convert.ToInt32(longWaitTime.Value);

                //Login Wait time
                XPathItem loginWaitTime = navigator.SelectSingleNode("Portal/RunSettings/loginWait");
                ConfigSettings.LoginWaitTime = Convert.ToInt32(loginWaitTime.Value);

                //Login Wait time
                XPathItem numberOfLoanTestDataRequired = navigator.SelectSingleNode("Portal/RunSettings/numberOfLoanTestDataRequired");
                ConfigSettings.NumberOfLoanTestDataRequired = Convert.ToInt32(numberOfLoanTestDataRequired.Value);

                //Jira url
                XPathItem jiraUrl = navigator.SelectSingleNode("Portal/RunSettings/JiraUrl");
                ConfigSettings.JiraUrl = jiraUrl.Value;

                //Jira username
                XPathItem jiraUsername = navigator.SelectSingleNode("Portal/RunSettings/JiraUsername");
                ConfigSettings.JiraUsername = jiraUsername.Value;

                //Jira password
                XPathItem jiraPassword = navigator.SelectSingleNode("Portal/RunSettings/JiraPassword");
                ConfigSettings.JiraPassword = jiraPassword.Value;

                #endregion CommonSettings

                #region AgentPortalSpecificSettings


                #endregion AgentPortalSpecificSettings

                #region CustomerPortalSpecificSettings


                #endregion CustomerPortalSpecificSettings
            }
        }
    }
}
