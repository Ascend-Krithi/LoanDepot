using System.Collections.Generic;

namespace AutomationFramework.Core.Models
{
    public class TestDataModel
    {
        private readonly Dictionary<string, TestCaseData> _testData = new Dictionary<string, TestCaseData>
        {
            { "TC01", new TestCaseData { Username = "standard_user", Password = "standard_user_password" } },
            { "TC02", new TestCaseData { Username = "invalid_user", Password = "invalid_password", ErrorMessage = "Epic sadface: Username and password do not match any user in this service" } },
            { "TC03", new TestCaseData { Username = "standard_user", Password = "standard_user_password", PageTitle = "PRODUCTS" } }
        };

        public TestCaseData GetTestData(string testCaseId)
        {
            return _testData[testCaseId];
        }
    }

    public class TestCaseData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string PageTitle { get; set; }
    }
}