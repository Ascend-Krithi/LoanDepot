using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

/// <summary>
/// PaymentsPage - Page Object Model for Make a Payment functionality in Customer Portal.
/// Implements locators and methods for payment date selection and late fee message verification.
/// Locators mapped from Servicing _ loanDepot_Date Selection for Payment.html and Servicing _ loanDepot_Payment Page.html
/// Test Cases: CUSTP-2005 TS-001 TC-001, TS-002 TC-001, TS-003 TC-001, TS-004 TC-001, TS-005 TC-001, TS-006 TC-001, TS-007 TC-001, TS-008 TC-001, TS-009 TC-001
/// </summary>
public class PaymentsPage
{
    private IWebDriver _driver;
    private ExtentTest test;
    private WebElementExtensionsPage webElementExtensions;

    /// <summary>
    /// Constructor for PaymentsPage
    /// </summary>
    public PaymentsPage(IWebDriver driver, ExtentTest test)
    {
        this._driver = driver;
        this.test = test;
        webElementExtensions = new WebElementExtensionsPage(driver, test);
        PageFactory.InitElements(driver, this);
    }

    #region Locators
    // Locators mapped from Servicing _ loanDepot_Date Selection for Payment.html
    // Make a Payment button
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Make a Payment')]")]
    public IWebElement MakeAPaymentBtn;
    // Add an Account button
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Add an Account')]")]
    public IWebElement AddAnAccountBtn;
    // Manage Accounts button
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Manage Accounts')]")]
    public IWebElement ManageAccountsBtn;
    // Edit Payment button
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Edit Payment')]")]
    public IWebElement EditPaymentBtn;
    // Confirm Payment button
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Confirm Payment')]")]
    public IWebElement ConfirmPaymentBtn;
    // Late Fee Message (Assumed XPath based on element name)
    [FindsBy(How = How.XPath, Using = "//div[contains(@class,'late-fee-message')]")]
    public IWebElement LateFeeMessageDiv;
    // Payment Date Picker (Assumed XPath)
    [FindsBy(How = How.XPath, Using = "//input[@type='date']")]
    public IWebElement PaymentDateInput;
    #endregion

    #region Methods
    /// <summary>
    /// Clicks the Make a Payment button
    /// </summary>
    public void ClickMakeAPayment()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment button (Servicing _ loanDepot_Date Selection for Payment.html)");
        MakeAPaymentBtn.Click();
    }

    /// <summary>
    /// Selects the payment date in the date picker
    /// </summary>
    /// <param name="date">Date string in MM/dd/yyyy format</param>
    public void SelectPaymentDate(string date)
    {
        test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {date} (Servicing _ loanDepot_Date Selection for Payment.html)");
        PaymentDateInput.Clear();
        PaymentDateInput.SendKeys(date);
    }

    /// <summary>
    /// Clicks the Confirm Payment button
    /// </summary>
    public void ClickConfirmPayment()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Clicking Confirm Payment button (Servicing _ loanDepot_Date Selection for Payment.html)");
        ConfirmPaymentBtn.Click();
    }

    /// <summary>
    /// Returns true if late fee message is displayed
    /// </summary>
    public bool IsLateFeeMessageDisplayed()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Checking for late fee message (Servicing _ loanDepot_Date Selection for Payment.html)");
        return LateFeeMessageDiv.Displayed;
    }

    /// <summary>
    /// Gets the late fee message text
    /// </summary>
    public string GetLateFeeMessageText()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Getting late fee message text (Servicing _ loanDepot_Date Selection for Payment.html)");
        return LateFeeMessageDiv.Text;
    }
    #endregion
}
