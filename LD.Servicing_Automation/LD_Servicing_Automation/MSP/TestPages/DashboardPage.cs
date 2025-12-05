using System;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework;
using System.Linq;
using System.Collections.Generic;

namespace LD_AgentPortal.Pages
{
    public class DashboardPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }

        WebElementExtensionsPage webElementExtensions = null;

        PaymentsPage payments = null;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DashboardPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            payments = new PaymentsPage(_driver, test);
        }

        #region Locators

        public By directorOptionLocBy = By.XPath("//section[@id='servicing']//a[text()='Director']");

        public By ifrAppIframeLocBy = By.CssSelector("iframe[id='ifr_APP']");

        public By modalLoadingIconLocBy = By.XPath("//div[@id='modalBackground' and @style='display: block;']");

        public By modalLoadingIconNotDisplayedLocBy = By.XPath("//div[@id='modalBackground' and @style='display: none;']");

        public By loanNumberMenuBarLocBy = By.CssSelector("div[id='LoanBar'] div[style='display:inline-block']");

        public By loanNumberTextboxLocBy = By.Id("LoanSelect");

        public By loanDataRetrieveIconLocBy = By.CssSelector("button[title='Retrieve']");

        public By workspaceLocBy = By.Id("workspace");

        public By smartButtonsMenuBarLocBy = By.Id("SmartButtons");

        public string smartButtonMenu = "//span[contains(@class,'smartBtn') and text()='<MENUNAME>']";

        public By screenTextboxLocBy = By.CssSelector("input[data-id='screenSel']");

        public By goButtonLocBy = By.Id("btnQuickNav");

        public By processTextfieldLocBy = By.CssSelector("input[title*='This field indicates a code that determines whether monetary transactions are applied to the loan']");

        public By processStopCodeChangeReasonTextfieldLocBy = By.CssSelector("input[title*='This field indicates the reason why the Process Stop was changed']");

        public By lastChangedDateLocBy = By.XPath("//span[text()='Q']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='T']//parent::span[@class='em-text f Hi ']/following-sibling::span[position()=1]//span");

        #region ELC3Screen

        public By dueDateLocBy = By.Id("f55");

        public By billGenerationDateLocBy = By.XPath("//span[text()='T']//following-sibling::span[position()=1][text()='H']//following-sibling::span[position()=1][text()='R']//parent::span[@class='em-text f ']/following-sibling::span[position()=1]//span");

        public string billGenerationDateFieldIndividualSpan = "//span[text()='T']//following-sibling::span[position()=1][text()='H']//following-sibling::span[position()=1][text()='R']//parent::span[@class='em-text f ']/following-sibling::span[position()=1]//span[<SPANNUMBER>]";

        public By totalAmountDueLocBy = By.XPath("//span[text()='P']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='L']//parent::span[@class='em-text f ']/preceding-sibling::span[position()=1]//span");

        public string totalAmountDueFieldIndividualSpan = "//span[text()='P']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='L']//parent::span[@class='em-text f ']/preceding-sibling::span[position()=1]//span[<SPANNUMBER>]";

        public By priorBillsUnpdLocBy = By.XPath("//span[text()='I']//following-sibling::span[position()=1][text()='P']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='L']//parent::span[@class='em-text f ']/preceding-sibling::span[position()=2]//span");

        public string priorBillsUnpdFieldIndividualSpan = "//span[text()='I']//following-sibling::span[position()=1][text()='P']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='L']//parent::span[@class='em-text f ']/preceding-sibling::span[position()=2]//span[<SPANNUMBER>]";

        public By minimumPmtFirstRecordLocBy = By.XPath("//span[text()='M']//following-sibling::span[position()=1][text()='P']//following-sibling::span[position()=1][text()='M']//following-sibling::span[position()=1][text()='T']//parent::span[@class='em-text f ']/following-sibling::span[position()=2]//input");

        public By minimumPmtLocBy = By.XPath("//span[text()='M']//following-sibling::span[position()=1][text()='P']//following-sibling::span[position()=1][text()='M']//following-sibling::span[position()=1][text()='T']//parent::span[@class='em-text f ']/following-sibling::span[position()=1]//input");

        #endregion ELC3Screen

        #region PMT2Screen

        public By grp_FieldLocBy = By.Id("f3");

        public By amtRecvd_FieldLocBy = By.XPath("//span[text()='R']//following-sibling::span[position()=1][text()='E']//following-sibling::span[position()=1][text()='G']//parent::span[@class='em-text f ']//following-sibling::span[position()=1]//input[@class='TermInp MSPFld font-med']");

        public By regpmt_FieldLocBy = By.XPath("//span[text()='R']//following-sibling::span[position()=1][text()='E']//following-sibling::span[position()=1][text()='G']//parent::span[@class='em-text f ']//following-sibling::span[position()=3]//input[@class='TermInp MSPFld font-med']");

        public By ovr_FieldLocBy = By.Id("f65");

        #endregion PMT2Screen

        #region PMTUScreen

        public By status_FieldLocBy = By.Id("f8");

        public By control_ItemCount_FieldLocBy = By.Id("f19");

        public By control_AmountReceived_FieldLocBy = By.Id("f20");

        #endregion PMTUScreen

        #region DENIScreen

        public By inputFieldToEnterMonthDifferenceBetweenCurrentAndDueDateLocBy = By.XPath("//span[text()='D']//following-sibling::span[position()=1][text()='A']//following-sibling::span[position()=1][text()='T']//following-sibling::span[position()=1][text()='E']//parent::span[@class='em-text f ']//following-sibling::span[2]//input[@class='TermInp MSPFld font-med']");

        public By inputFieldToEnterReasonLocBy = By.XPath("//span[text()='A']//following-sibling::span[position()=1][text()='S']//following-sibling::span[position()=1][text()='O']//following-sibling::span[position()=1][text()='N']//parent::span[@class='em-text f ']//following-sibling::span[1]//input[@class='TermInp MSPFld font-med']");

        #endregion DENIScreen

        #endregion Locators

        #region Services     



        #endregion Services        
    }
}
