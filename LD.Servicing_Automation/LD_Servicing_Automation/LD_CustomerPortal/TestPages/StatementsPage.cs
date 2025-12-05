using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using iTextSharp.text.xml.simpleparser;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using Microsoft.SqlServer.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LD_CustomerPortal.TestPages
{
    //documents class 
    public class Document
    {
        public string DocumentDate { get; set; }
        public string DocumentType { get; set; }

        public override string ToString()
        {
            return $"{DocumentDate}:{DocumentType}";
        }
    }
/// <summary>
/// Class to handle all statements and documents functionality
/// </summary>
    public class StatementsPage
    {

        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
       public StatementsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators
       public By pageTitleLocBy = By.Id("spidPviewSedmContainer");
       public By docCategoryDropdownLocBy = By.Id("ddlIdDocCategory");
       public By startDateCalanderLocBy = By.XPath("//*[@id='txtIdstartDate']/../following-sibling::div");
       public By endDateCalanderLocBy = By.XPath("//*[@id='txtIdendDate']/../following-sibling::div");
       public By startDateLocBy = By.Id("txtIdstartDate");
       public By endDateLocBy = By.Id("txtIdendDate");
       public By searchBtnLocBy = By.Id("btnIdSearchSubmit");
       public By statementsListLocBy = By.Id("tblIdSedmList");
       public By noDocumentsAlert = By.Id("pIdNoDocumentsAlert");

       string docCategory = "//*[contains(@id,'mat-option')]/span[contains(.,'{0}')]";
       string noDocsMessage = "There are no statements or documents available in the date range you selected.";
        string viewDocument = "//*[@id='tblIdSedmList']//tr[{0}]//button";

        #endregion

        #region methods
        /// <summary>
        /// Get current documents lists showing in DOM
        /// </summary>
        /// <returns>List<Document></returns>
        public List<Document> GetDocumentsList()
        {
            List<Document> documents =new List<Document> ();
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForElement(_driver, statementsListLocBy);
                var tablerows = _driver.FindElement(statementsListLocBy)
                    .FindElements(By.TagName("tr"));

                int index = 0;
                foreach (var row in tablerows)
                {
                    if (index == tablerows.Count - 1)
                        break;

                    documents.Add(new Document()
                    {
                        DocumentDate = _driver.FindElement(By.Id($"spPaymentDate_{index}")).Text,
                        DocumentType = _driver.FindElement(By.Id($"spIdDocType{index}")).Text,
                    });

                    index++;

                }
            }catch(Exception ex) 
            {
                ReportingMethods.Log(test, "No Documents Found");
            }
            return documents;
        }

        /// <summary>
        /// Search for the documents with selected category and assert them with their respective sub-category
        /// </summary>
        /// <param name="category">category of document</param>
        /// <returns>true if documents exists else false</returns>
        public bool SearchForDocumentCategory(string category)
        {
            bool areDocumentsExists = false;
            try
            {
                webElementExtensions.ClickElement(_driver, docCategoryDropdownLocBy);
                By docCategoryElement = By.XPath(string.Format(docCategory, category));
                webElementExtensions.WaitForElement(_driver, docCategoryElement);
                webElementExtensions.ClickElement(_driver, docCategoryElement);
                webElementExtensions.ClickElement(_driver, searchBtnLocBy);
                var documents = GetDocumentsList();
                reportLogger.TakeScreenshot(test, $"Statements and Documents : {category}");
                //define all sub types

                List<string> allTypes = new List<string>();

                allTypes.AddRange(Constants.Letters);
                allTypes.AddRange(Constants.OtherTypes);
                allTypes.AddRange(Constants.TaxDocs);

                bool isAllMatching;

                isAllMatching = false;
                //check listed document category matching with its sub type
                if (documents.Count > 0)
                {
                    areDocumentsExists = true;

                    switch (category)
                    {
                        case "Monthly Statement":
                        case "Escrow Statement":
                            isAllMatching = documents.TrueForAll(d => d.DocumentType == category);
                            break;

                        case "Letters":
                            isAllMatching = documents.TrueForAll(d => Constants.Letters.Contains(d.DocumentType));
                            break;

                        case "Tax Documents":
                            isAllMatching = documents.TrueForAll(d => Constants.TaxDocs.Contains(d.DocumentType));
                            break;

                        default:
                            isAllMatching = documents.TrueForAll(d => allTypes.Contains(d.DocumentType));
                            break;
                    }
                    ReportingMethods.LogAssertionTrue(test, isAllMatching, $"All Document categories should match with selected category: {category} ");

                }
                else
                {
                    bool isNoDocsShowing = webElementExtensions.IsElementDisplayed(_driver, noDocumentsAlert);
                    ReportingMethods.LogAssertionTrue(test, isNoDocsShowing, "No Documents Alert should show if no documents");
                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror in Statement and Documents, {e.Message}");
            }
            return areDocumentsExists;

        }
        /// <summary>
        /// Click on First View Document available in Documents List
        /// </summary>

        public void CheckViewDocument()
        {
           bool isDocsExists= SearchForDocumentCategory("All Document Types");
           bool isDocOpened = false;
            try
            {
                if (isDocsExists)
                {
                    //Open any document is good enough, hence clicking on first row Document
                    var tabsBefore = _driver.WindowHandles;
                    var mainHandle = _driver.CurrentWindowHandle;
                    reportLogger.TakeScreenshot(test, $"View Document Button");
                    By viewDocumentLocBy = By.XPath(string.Format(viewDocument, 1));
                    webElementExtensions.ClickElementUsingJavascript(_driver, viewDocumentLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForPageLoad(_driver);
                    //Wait for new tab
                    webElementExtensions.WaitForNewTabWindow(_driver, tabsBefore.Count + 1, ConfigSettings.LongWaitTime);
                    var tabsAfter = _driver.WindowHandles;

                    ReportingMethods.LogAssertionTrue(test, tabsAfter.Count > tabsBefore.Count, "Document should be opened in new tab");
                    if (tabsAfter.Count > 0)
                    {
                        bool isSwitched = false;
                        foreach (var tab in tabsAfter)
                        {
                            try
                            {
                                webElementExtensions.SwitchToTab(_driver, tab);
                                isSwitched = true;
                                if (tab != mainHandle && _driver.Url.Contains("borrower-documents"))
                                {
                                    webElementExtensions.WaitForPageLoad(_driver, pollingInterval: 2);
                                    isDocOpened = true;
                                    reportLogger.TakeScreenshot(test, $"View Document in new Tab");
                                    ReportingMethods.LogAssertionContains(test, "borrower-documents", _driver.Url, "Documents Url should have borrowed Documents");
                                    _driver.Close();
                                }
                            }
                            catch(Exception ex)
                            {
                                if (isSwitched) { _driver.Close(); }
                                //Printing in logs is enough and no need to fail the test since somwtimes haidden tabs causingthe failure while switching
                                ReportingMethods.Log(test, $"Exception while switching {ex.Message}");
                            }
                        }
                        webElementExtensions.SwitchToTab(_driver, mainHandle);
                    }
                    ReportingMethods.LogAssertionTrue(test, isDocOpened, "Document View should be Successful");

                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while viewing document, {e.Message}");
            }
        }

        #endregion
    }
}
