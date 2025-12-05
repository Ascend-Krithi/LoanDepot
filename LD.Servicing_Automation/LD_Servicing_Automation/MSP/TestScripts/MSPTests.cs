using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;

namespace MSP
{
    [TestClass]
    public class MSPTests:BasePage
    {
        public static string top100LoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueriesForMSP.GetTop100LoanLevelData));
        public static string helocDelinquentPlusLoanDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueriesForMSP.GetHelocDelinquentPlusLoanData));
        public static string delinquentPlusLoanDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueriesForMSP.GetDelinquentPlusLoanData));
        public TestContext TestContext
        {
            set;
            get;
        }

        [ClassInitialize]
        public static void TestClassInitialize(TestContext TestContext)
        {

        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ObjectInitialization

        DashboardPage dashboard = null;
        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        DBconnect dBconnect = null;
        LD_AutomationFramework.Pages.PaymentsPage payments = null;
        ReportLogger reportLogger = null;
        List<Hashtable> loanLevelData = null;
        JiraManager jiraManager = null;

        #endregion ObjectInitialization

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            dashboard = new DashboardPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            dBconnect = new DBconnect(test, "MelloServETL");
            payments = new LD_AutomationFramework.Pages.PaymentsPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
            loanLevelData = new List<Hashtable>();
            jiraManager = new JiraManager(test);
        }

        [TestMethod]
        [Description("Set Hard Stop for a loan in MSP")]
        [TestCategory("MSP")]
        public void SetHardStopCodeFunctionality()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(top100LoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);            
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
            By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
            webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);            
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.SAF1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);            
            webElementExtensions.EnterText(_driver, dashboard.processTextfieldLocBy, Constants.ProcessStopCodes.Code2);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
        }

        [TestMethod]
        [Description("Set Soft Stop for a loan in MSP")]
        [TestCategory("MSP")]
        public void SetSoftStopCodeFunctionality()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(top100LoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
            By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
            webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.SAF1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.EnterText(_driver, dashboard.processTextfieldLocBy, Constants.ProcessStopCodes.Code2);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
        }

        [TestMethod]
        [Description("Set Repayment loan in MSP")]
        [TestCategory("MSP")]
        public void Defaults_Repayment()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(top100LoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
            By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
            webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.SAF1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.EnterText(_driver, dashboard.processTextfieldLocBy, Constants.ProcessStopCodes.Code2);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
        }

        [TestMethod]
        [Description("Set Forbearance loan in MSP")]
        [TestCategory("MSP")]
        public void Defaults_Forbearance()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(top100LoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
            By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
            webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.SAF1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.EnterText(_driver, dashboard.processTextfieldLocBy, Constants.ProcessStopCodes.Code2);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
        }

        [TestMethod]
        [Description("Set Modification Trial loan in MSP")]
        [TestCategory("MSP")]
        public void Defaults_ModificationTrial()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(top100LoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
            By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
            webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.SAF1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.EnterText(_driver, dashboard.processTextfieldLocBy, Constants.ProcessStopCodes.Code2);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
        }

        [TestMethod]
        [Description("Convert Delinquent Plus loans into Past Due in MSP")]
        [TestCategory("MSP")]
        public void RollForwardDueDate_ConvertDelinquentPlusLoansToPastDue()
        {
            int loanDataCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(delinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                log.Info("Selected Loan " + loanDataCount + 1 + ".");
                loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                string todayDate = DateTime.Now.ToString();
                todayDate = webElementExtensions.DateTimeConverter(_driver, todayDate, "mm/dd/yy to m/dd/yyyy");
                todayDate = todayDate.Split(' ')[0];
                webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DENI);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.EnterText(_driver, dashboard.inputFieldToEnterMonthDifferenceBetweenCurrentAndDueDateLocBy, Constants.ProcessStopCodes.Code2);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                loanDataCount++;
            }
        }

        [TestMethod]
        [Description("Convert Delinquent Plus loans into Delinquent in MSP")]
        [TestCategory("MSP")]
        public void RollForwardDueDate_ConvertDelinquentPlusLoansToDelinquent()
        {
            int loanDataCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(delinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                log.Info("Selected Loan " + loanDataCount + 1 + ".");
                loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                string todayDate = DateTime.Now.ToString();
                todayDate = webElementExtensions.DateTimeConverter(_driver, todayDate, "mm/dd/yy to m/dd/yyyy");
                todayDate = todayDate.Split(' ')[0];
                webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DENI);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.EnterText(_driver, dashboard.inputFieldToEnterMonthDifferenceBetweenCurrentAndDueDateLocBy, Constants.ProcessStopCodes.Code2);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                loanDataCount++;
            }
        }

        [TestMethod]
        [Description("Convert Delinquent Plus loans into On time in MSP")]
        [TestCategory("MSP")]
        public void RollForwardDueDate_ConvertDelinquentPlusLoansToOntime()
        {
            int loanDataCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty,
                typeOfMortgage = string.Empty, propertyFullAddress = string.Empty;

            #region TestData

            List<string> loanSummaryDetails = null;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName,
                                                                 Constants.LoanLevelDataColumns.NextPaymentDueDate,
                                                                 Constants.LoanLevelDataColumns.PropertyAddress,
                                                                 Constants.LoanLevelDataColumns.PropertyCity,
                                                                 Constants.LoanLevelDataColumns.PropertyState,
                                                                 Constants.LoanLevelDataColumns.PropertyZip,
                                                                 Constants.LoanLevelDataColumns.ProductType};

            loanLevelData = commonServices.GetLoanDataFromDatabase(delinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
            webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
            webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
            webElementExtensions.SwitchTabs(_driver, 1);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
            webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
            while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                log.Info("Selected Loan " + loanDataCount + 1 + ".");
                loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                string todayDate = DateTime.Now.ToString();
                todayDate = webElementExtensions.DateTimeConverter(_driver, todayDate, "mm/dd/yy to m/dd/yyyy");
                todayDate = todayDate.Split(' ')[0];
                webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DENI);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.EnterText(_driver, dashboard.inputFieldToEnterMonthDifferenceBetweenCurrentAndDueDateLocBy, Constants.ProcessStopCodes.Code2);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.processStopCodeChangeReasonTextfieldLocBy, "PA");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.DLQ1);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                loanDataCount++;
            }
        }

        [TestMethod]
        [Description("Roll forward Heloc Delinquent Plus loans to On Time")]
        [TestCategory("MSP"), TestCategory("MSP_Heloc")]
        public void HelocDataConditioning_DelinquentPlusToOnTime()
        {
            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            int loanDataCount = 0;
            double totalAmountDueForAllLoansUsed = 0;
            string loanNumber = string.Empty, nextPaymentDueDate = string.Empty, result = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.NextPaymentDueDate };
            loanLevelData = commonServices.GetLoanDataFromDatabase(helocDelinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForElement(_driver, dashboard.ifrAppIframeLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
                webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
                while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    log.Info("Selected Loan " + loanDataCount + 1 + ".");
                    loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                    webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber, true, "Loan Number", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                    By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                    webElementExtensions.WaitForVisibilityOfElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                    webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.ELC3);
                    log.Info("Navigated to "+ Constants.MSPScreenNames.ELC3 + "screen.");
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.dueDateLocBy);

                    string dueDateValueFromApplication = webElementExtensions.GetElementAttribute(_driver, dashboard.dueDateLocBy, Constants.ElementAttributes.Value);
                    dueDateValueFromApplication = webElementExtensions.DateTimeConverter(_driver, dueDateValueFromApplication, "mm/dd/yy to m/d/yyyy");
                    string billGenerationDate = string.Empty;

                    int billGenerationDateFieldSpanCount = _driver.FindElements(dashboard.billGenerationDateLocBy).Count;
                    for (int index = 1; index <= billGenerationDateFieldSpanCount; index++)
                    {
                        billGenerationDate = billGenerationDate + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.billGenerationDateFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                    }
                    billGenerationDate = webElementExtensions.DateTimeConverter(_driver, billGenerationDate, "mm/dd/yy to m/d/yyyy");
                    billGenerationDate = billGenerationDate.Split(' ')[0];
                    dueDateValueFromApplication = dueDateValueFromApplication.Split(' ')[0];
                    DateTime billGenDateNewFormat = new DateTime(Convert.ToInt32(billGenerationDate.Split('/')[2]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[0]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[1]));
                    DateTime dueDateFromAppNewFormat = new DateTime(Convert.ToInt32(dueDateValueFromApplication.Split('/')[2]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[0]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[1]));                    
                    test.Log(Status.Info, "Bill generation date - " + billGenDateNewFormat + ". Due date - " + dueDateFromAppNewFormat + ".");
                    
                    string totalAmountDueForALoan = string.Empty;
                    if (billGenDateNewFormat > dueDateFromAppNewFormat)
                    {
                        int totalAmountDueFieldSpanCount = _driver.FindElements(dashboard.totalAmountDueLocBy).Count;
                        for (int index = 1; index <= totalAmountDueFieldSpanCount; index++)
                        {
                            totalAmountDueForALoan = totalAmountDueForALoan + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.totalAmountDueFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                        }
                    }
                    else
                    {
                        int totalAmountDueFieldSpanCount = _driver.FindElements(dashboard.priorBillsUnpdLocBy).Count;
                        for (int index = 1; index <= totalAmountDueFieldSpanCount; index++)
                        {
                            totalAmountDueForALoan = totalAmountDueForALoan + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.priorBillsUnpdFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                        }
                    }
                    if (!totalAmountDueForALoan.Equals(".00"))
                    {
                        webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMT2);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        log.Info("Navigated to " + Constants.MSPScreenNames.PMT2 + "screen.");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.grp_FieldLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.grp_FieldLocBy, "4TA");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForALoan, true, "Amount Received", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.regpmt_FieldLocBy, totalAmountDueForALoan);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.ovr_FieldLocBy, "1");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);

                        totalAmountDueForALoan = totalAmountDueForALoan.Replace(",", "");
                        totalAmountDueForAllLoansUsed = totalAmountDueForAllLoansUsed + Convert.ToDouble(totalAmountDueForALoan);
                        loansWithSuccessfullPaymentSetup.Add(loanNumber);
                    }
                    loanDataCount++;                    
                }
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMTU);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.control_ItemCount_FieldLocBy, loansWithSuccessfullPaymentSetup.Count.ToString(), true, "Item Count", true);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForAllLoansUsed.ToString(), true, "Amount Received", true);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.status_FieldLocBy, "R");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                log.Info("Released the changes.");
            }
            catch (Exception ex)
            {
                log.Error("Failed while MSP data conditioning: " + ex.Message);
            }
            finally
            {
                foreach (string loan in loansWithSuccessfullPaymentSetup)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = loansWithSuccessfullPaymentSetup.Count;
                test.Log(Status.Pass, "Data conditioning done for the following loan numbers - " + result + ". Count = " + count + ".");
                test.Log(Status.Info, "Total amount - " + totalAmountDueForAllLoansUsed + ".");

            }
        }

        [TestMethod]
        [Description("Roll forward Heloc Delinquent Plus loans to Past Due")]
        [TestCategory("MSP"), TestCategory("MSP_Heloc")]
        public void HelocDataConditioning_DelinquentPlusToPastDue()
        {
            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            int loanDataCount = 0;
            double totalAmountDueForAllLoansUsed = 0;
            string loanNumber = string.Empty, nextPaymentDueDate = string.Empty, result = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.NextPaymentDueDate };
            loanLevelData = commonServices.GetLoanDataFromDatabase(helocDelinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForElement(_driver, dashboard.ifrAppIframeLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
                webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
                while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                    webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber, true, "Loan Number", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                    By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                    webElementExtensions.WaitForVisibilityOfElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                    webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.ELC3);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.dueDateLocBy);

                    string dueDateValueFromApplication = webElementExtensions.GetElementAttribute(_driver, dashboard.dueDateLocBy, Constants.ElementAttributes.Value);
                    dueDateValueFromApplication = webElementExtensions.DateTimeConverter(_driver, dueDateValueFromApplication, "mm/dd/yy to m/d/yyyy");
                    string billGenerationDate = string.Empty;

                    int billGenerationDateFieldSpanCount = _driver.FindElements(dashboard.billGenerationDateLocBy).Count;
                    for (int index = 1; index <= billGenerationDateFieldSpanCount; index++)
                    {
                        billGenerationDate = billGenerationDate + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.billGenerationDateFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                    }
                    billGenerationDate = webElementExtensions.DateTimeConverter(_driver, billGenerationDate, "mm/dd/yy to m/d/yyyy");
                    billGenerationDate = billGenerationDate.Split(' ')[0];
                    dueDateValueFromApplication = dueDateValueFromApplication.Split(' ')[0];
                    DateTime billGenDateNewFormat = new DateTime(Convert.ToInt32(billGenerationDate.Split('/')[2]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[0]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[1]));
                    DateTime dueDateFromAppNewFormat = new DateTime(Convert.ToInt32(dueDateValueFromApplication.Split('/')[2]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[0]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[1]));
                    test.Log(Status.Info, "Bill generation date - " + billGenDateNewFormat + ". Due date - " + dueDateFromAppNewFormat + ".");

                    string totalAmountDueForALoan = string.Empty, priorBillsUnpaid = string.Empty;
                    double totalMinimumPmtAmountDueForALoan = 0, monthlyAmountDueForALoan = 0;
                    int totalAmountDueFieldSpanCount = _driver.FindElements(dashboard.priorBillsUnpdLocBy).Count;
                    for (int index = 1; index <= totalAmountDueFieldSpanCount; index++)
                    {
                        priorBillsUnpaid = priorBillsUnpaid + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.priorBillsUnpdFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                    }
                    if (billGenDateNewFormat > dueDateFromAppNewFormat)
                    {
                        totalAmountDueForALoan = priorBillsUnpaid;
                    }
                    else
                    {   
                        priorBillsUnpaid = priorBillsUnpaid.Replace(",", "");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.F5);
                        webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.minimumPmtLocBy);
                        monthlyAmountDueForALoan = Convert.ToDouble(webElementExtensions.GetElementAttribute(_driver, dashboard.minimumPmtLocBy, Constants.ElementAttributes.Value).Trim(' '));
                        totalMinimumPmtAmountDueForALoan = Convert.ToDouble(priorBillsUnpaid) - monthlyAmountDueForALoan;
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                        totalAmountDueForALoan = totalMinimumPmtAmountDueForALoan.ToString();
                    }
                    if (!totalAmountDueForALoan.Equals(".00"))
                    {
                        webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMT2);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.grp_FieldLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.grp_FieldLocBy, "4TA");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForALoan, true, "Amount Received", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.regpmt_FieldLocBy, totalAmountDueForALoan);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.ovr_FieldLocBy, "1");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);

                        totalAmountDueForALoan = totalAmountDueForALoan.Replace(",", "");
                        totalAmountDueForAllLoansUsed = totalAmountDueForAllLoansUsed + Convert.ToDouble(totalAmountDueForALoan);
                        loansWithSuccessfullPaymentSetup.Add(loanNumber);
                    }
                    loanDataCount++;
                }
                test.Log(Status.Info, "Number of loans used - " + loanDataCount + ", Total amount - " + totalAmountDueForAllLoansUsed + ".");
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMTU);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.control_ItemCount_FieldLocBy, loansWithSuccessfullPaymentSetup.Count.ToString(), true, "Item Count", true, false, true);
                webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForAllLoansUsed.ToString());
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.status_FieldLocBy, "R");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while MSP data conditioning: " + ex.Message);
            }
            finally
            {
                foreach (string loan in loansWithSuccessfullPaymentSetup)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = loansWithSuccessfullPaymentSetup.Count;
                test.Log(Status.Pass, "Data conditioning done for the following loan numbers - " + result + ". Count = " + count + ".");
                test.Log(Status.Info, "Total amount - " + totalAmountDueForAllLoansUsed + ".");

            }
        }

        [TestMethod]
        [Description("Roll forward Heloc Delinquent Plus loans to Delinquent")]
        [TestCategory("MSP"), TestCategory("MSP_Heloc")]
        public void HelocDataConditioning_DelinquentPlusToDelinquent()
        {
            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            int loanDataCount = 0;
            double totalAmountDueForAllLoansUsed = 0;
            string loanNumber = string.Empty, nextPaymentDueDate = string.Empty, result = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.NextPaymentDueDate };
            loanLevelData = commonServices.GetLoanDataFromDatabase(helocDelinquentPlusLoanDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForElement(_driver, dashboard.ifrAppIframeLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.APP_Frame);
                webElementExtensions.WaitForElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.directorOptionLocBy);
                webElementExtensions.SwitchToFrame(_driver, Constants.MSPFrameNames.DIR_Frame);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.modalLoadingIconNotDisplayedLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.smartButtonsMenuBarLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanNumberTextboxLocBy);
                while (loanDataCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    loanNumber = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    nextPaymentDueDate = loanLevelData[loanDataCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
                    webElementExtensions.EnterText(_driver, dashboard.loanNumberTextboxLocBy, loanNumber, true, "Loan Number", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, dashboard.loanDataRetrieveIconLocBy);
                    By smartButtonMenuLocBy = By.XPath(dashboard.smartButtonMenu.Replace("<MENUNAME>", Constants.MSPScreenNames.DLQ1));
                    webElementExtensions.WaitForVisibilityOfElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.ClickElement(_driver, smartButtonMenuLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Home);
                    webElementExtensions.ScrollIntoView(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.screenTextboxLocBy);
                    webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.ELC3);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.dueDateLocBy);

                    string dueDateValueFromApplication = webElementExtensions.GetElementAttribute(_driver, dashboard.dueDateLocBy, Constants.ElementAttributes.Value);
                    dueDateValueFromApplication = webElementExtensions.DateTimeConverter(_driver, dueDateValueFromApplication, "mm/dd/yy to m/d/yyyy");
                    string billGenerationDate = string.Empty;

                    int billGenerationDateFieldSpanCount = _driver.FindElements(dashboard.billGenerationDateLocBy).Count;
                    for (int index = 1; index <= billGenerationDateFieldSpanCount; index++)
                    {
                        billGenerationDate = billGenerationDate + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.billGenerationDateFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                    }
                    billGenerationDate = webElementExtensions.DateTimeConverter(_driver, billGenerationDate, "mm/dd/yy to m/d/yyyy");
                    billGenerationDate = billGenerationDate.Split(' ')[0];
                    dueDateValueFromApplication = dueDateValueFromApplication.Split(' ')[0];
                    DateTime billGenDateNewFormat = new DateTime(Convert.ToInt32(billGenerationDate.Split('/')[2]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[0]),
                                                  Convert.ToInt32(billGenerationDate.Split('/')[1]));
                    DateTime dueDateFromAppNewFormat = new DateTime(Convert.ToInt32(dueDateValueFromApplication.Split('/')[2]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[0]),
                                                  Convert.ToInt32(dueDateValueFromApplication.Split('/')[1]));
                    test.Log(Status.Info, "Bill generation date - " + billGenDateNewFormat + ". Due date - " + dueDateFromAppNewFormat + ".");

                    string totalAmountDueForALoan = string.Empty;
                    nextPaymentDueDate = nextPaymentDueDate.Split(' ')[0];
                    dueDateValueFromApplication = dueDateValueFromApplication.Split(' ')[0];
                    DateTime nextPaymentDueDateNewFormat = new DateTime(Convert.ToInt32(nextPaymentDueDate.Split('/')[2]),
                                                  Convert.ToInt32(nextPaymentDueDate.Split('/')[0]),
                                                  Convert.ToInt32(nextPaymentDueDate.Split('/')[1]));

                    int numberOfMonths = Math.Abs((dueDateFromAppNewFormat.Year - nextPaymentDueDateNewFormat.Year) * 12 + dueDateFromAppNewFormat.Month - nextPaymentDueDateNewFormat.Month);
                    
                    double totalMinimumPmtAmountDueForALoan = 0, monthlyAmountDueForALoan = 0;
                    string priorBillsUnpaid = string.Empty;
                    int minimumPmtRecordCount = 0;
                    int totalAmountDueFieldSpanCount = _driver.FindElements(dashboard.priorBillsUnpdLocBy).Count;
                    for (int index = 1; index <= totalAmountDueFieldSpanCount; index++)
                    {
                        priorBillsUnpaid = priorBillsUnpaid + webElementExtensions.GetElementText(_driver, By.XPath(dashboard.priorBillsUnpdFieldIndividualSpan.Replace("<SPANNUMBER>", index.ToString())));
                    }
                    priorBillsUnpaid = priorBillsUnpaid.Replace(",", "");
                    if (billGenDateNewFormat > dueDateFromAppNewFormat)
                    {                        
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.F5);
                        webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.minimumPmtLocBy);
                        monthlyAmountDueForALoan = Convert.ToDouble(webElementExtensions.GetElementAttribute(_driver, dashboard.minimumPmtLocBy, Constants.ElementAttributes.Value).Trim(' '));                        
                        totalMinimumPmtAmountDueForALoan = Convert.ToDouble(priorBillsUnpaid) - monthlyAmountDueForALoan;
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                    }
                    else
                    {
                        while (minimumPmtRecordCount < 2)
                        {
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.F5);
                            webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.minimumPmtLocBy);
                            monthlyAmountDueForALoan = Convert.ToDouble(webElementExtensions.GetElementAttribute(_driver, dashboard.minimumPmtLocBy, Constants.ElementAttributes.Value).Trim(' '));
                            totalMinimumPmtAmountDueForALoan = totalMinimumPmtAmountDueForALoan + monthlyAmountDueForALoan;
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                            webElementExtensions.WaitForElement(_driver, dashboard.billGenerationDateLocBy);
                            minimumPmtRecordCount++;
                        }
                        totalMinimumPmtAmountDueForALoan = Convert.ToDouble(priorBillsUnpaid) - totalMinimumPmtAmountDueForALoan;
                    }
                    totalAmountDueForALoan = totalMinimumPmtAmountDueForALoan.ToString();
                    if (!totalAmountDueForALoan.Equals(".00"))
                    {
                        webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMT2);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.grp_FieldLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.grp_FieldLocBy, "4TA");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForALoan.ToString(), true, "Amount Received", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.regpmt_FieldLocBy, totalAmountDueForALoan.ToString());
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.EnterText(_driver, dashboard.ovr_FieldLocBy, "1");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);

                        totalAmountDueForALoan = totalAmountDueForALoan.Replace(",", "");
                        totalAmountDueForAllLoansUsed = totalAmountDueForAllLoansUsed + Convert.ToDouble(totalAmountDueForALoan);
                        loansWithSuccessfullPaymentSetup.Add(loanNumber);
                    }
                    loanDataCount++;
                }
                webElementExtensions.EnterText(_driver, dashboard.screenTextboxLocBy, Constants.MSPScreenNames.PMTU);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.control_ItemCount_FieldLocBy, loansWithSuccessfullPaymentSetup.Count.ToString(), true, "Item Count", true, false, true);
                webElementExtensions.EnterText(_driver, dashboard.amtRecvd_FieldLocBy, totalAmountDueForAllLoansUsed.ToString());
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
                webElementExtensions.EnterText(_driver, dashboard.status_FieldLocBy, "R");
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.modalLoadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while MSP data conditioning: " + ex.Message);
            }
            finally
            {
                foreach (string loan in loansWithSuccessfullPaymentSetup)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = loansWithSuccessfullPaymentSetup.Count;
                test.Log(Status.Pass, "Data conditioning done for the following loan numbers - " + result + ". Count = " + count + ".");
                test.Log(Status.Info, "Total amount - " + totalAmountDueForAllLoansUsed + ".");

            }
        }
    }
}
