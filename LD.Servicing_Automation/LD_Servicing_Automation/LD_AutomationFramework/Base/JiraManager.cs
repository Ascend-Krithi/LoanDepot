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
    public class JiraManager
    {
        private RestManager restManager { get; set; }
        private ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public JiraManager(ExtentTest test)
        {
            string jiraPassword = new EncryptionManager(test).DecodeText(ConfigSettings.JiraPassword);
            restManager = new RestManager(ConfigSettings.JiraUsername, jiraPassword, test, ConfigSettings.JiraUrl);
            restManager.SecurityProtocolType = System.Net.SecurityProtocolType.Tls12;
            this.test = test;
        }
        public enum JiraResource
        {
            project,
            myself,
            execution,
            issue,
            versions,
            cycle,
            execute,
            attachment,
            updateBulkStatus
        }

        public struct JiraApiResource
        {
            public const string Jira_Uri = "api/latest";
            public const string Xray_Uri1 = "raven/1.0";
            public const string Xray_Uri2 = "raven/2.0";
        }
        public enum JiraJsonKeys
        {
            key,
            name,
            id,            
            folderId,
            folderName
        }
        public enum JiraResultStatus
        {
            Pass = 1,
            Fail = 2,
            WIP = 3,
            UNEXECUTED = -1
        }
        public string GetProjects()
        {
            string projectsString = restManager.GetMethod(JiraApiResource.Jira_Uri, JiraResource.project.ToString(), true);
            return projectsString;
        }
        public string GetVersions(string projectID)
        {
            string versionIdsString = restManager.GetMethod(JiraApiResource.Jira_Uri, JiraResource.project + "/" + projectID + "/" + JiraResource.versions, true);
            return versionIdsString;
        }
        public string GetIssueId(string issueKey)
        {
            string issueIdString = restManager.GetMethod(JiraApiResource.Jira_Uri, JiraResource.issue + "/" + issueKey, true);
            return issueIdString;
        }
        
        public string GetFolderId(string projectId, string versionId, string cycleID)
        {
            string data = "projectId=<projectID>&versionId=<versionID>".Replace("<projectID>", projectId).Replace("<versionID>", versionId);
            string folderId = restManager.GetMethod(JiraApiResource.Xray_Uri1, JiraResource.cycle + "?" + cycleID + "/folders?" + data, true);
            return folderId;
        }
        
        public void UpdateXrayTestCasesWithExecutionStatus(List<string> xrayTestCaseIdList, string projectCode, string folderName, string executionStatus, string[] fileNamesToAttach = null)
        {
            string projectsJsonContent = string.Empty, folderJsonContent = string.Empty, executionJsonContent = string.Empty,
            projectId = string.Empty, folderId = string.Empty, executionIds = string.Empty, execUpdateResponse = string.Empty,
            attachmentResponse = string.Empty;

            projectsJsonContent = GetProjects();
            
        }
    }
}
