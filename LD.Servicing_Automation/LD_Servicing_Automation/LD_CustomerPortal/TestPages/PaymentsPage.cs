using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

/// <summary>
/// PaymentsPage - Page Object Model for 'Make a Payment' functionality.
/// Strictly follows KB skeleton and maps locators from Servicing _ loanDepot_Payment Page.html and Servicing _ loanDepot_Date Selection for Payment.html.
/// </summary>
public class PaymentsPage
{
    private IWebDriver _driver;
    private ExtentTest test;
    private WebElementExtensionsPage webElementExtensions;

    /// <summary>
    /// Constructor initializes driver, test, and webElementExtensions.
    /// </summary>
    public PaymentsPage(IWebDriver driver, ExtentTest test)
    {
        this._driver = driver;
        this.test = test;
        webElementExtensions = new WebElementExtensionsPage(driver, test);
        PageFactory.InitElements(driver, this);
    }

    #region Locators
    // Locators mapped from Servicing _ loanDepot_Payment Page.html
    // [Source: LOCATORS JSON]
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Make a Payment')]")]
    private IWebElement makeAPaymentBtn; // Servicing _ loanDepot_Payment Page.html

    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Manage Autopay')]")]
    private IWebElement manageAutopayBtn; // Servicing _ loanDepot_Payment Page.html

    // Locators mapped from Servicing _ loanDepot_Date Selection for Payment.html
    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Add an Account')]")]
    private IWebElement addAccountBtn; // Servicing _ loanDepot_Date Selection for Payment.html

    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Manage Accounts')]")]
    private IWebElement manageAccountsBtn; // Servicing _ loanDepot_Date Selection for Payment.html

    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Edit Payment')]")]
    private IWebElement editPaymentBtn; // Servicing _ loanDepot_Date Selection for Payment.html

    [FindsBy(How = How.XPath, Using = "//button[contains(text(),'Confirm Payment')]")]
    private IWebElement confirmPaymentBtn; // Servicing _ loanDepot_Date Selection for Payment.html

    // Payment amount input (plausible XPath, as element_count: 0)
    [FindsBy(How = How.XPath, Using = "//input[@aria-label='Payment Amount']")]
    private IWebElement paymentAmountInput; // Assumed from context

    // Payment date input (plausible XPath, as element_count: 0)
    [FindsBy(How = How.XPath, Using = "//input[@aria-label='Payment Date']")]
    private IWebElement paymentDateInput; // Assumed from context

    // Late fee message (plausible XPath, as element_count: 0)
    [FindsBy(How = How.XPath, Using = "//div[contains(@class,'late-fee-message')]")]
    private IWebElement lateFeeMessageDiv; // Assumed from context
    #endregion

    #region Methods
    /// <summary>
    /// Clicks 'Make a Payment' button.
    /// </summary>
    public void ClickMakeAPayment()
    {
        webElementExtensions.WaitForElementClickable(_driver, By.XPath("//button[contains(text(),'Make a Payment')]"), ConfigSettings.WaitTime);
        makeAPaymentBtn.Click();
        test.Log(AventStack.ExtentReports.Status.Info, "Clicked 'Make a Payment' button.");
    }

    /// <summary>
    /// Enters payment amount.
    /// </summary>
    public void EnterPaymentAmount(string amount)
    {
        webElementExtensions.WaitForElement(_driver, By.XPath("//input[@aria-label='Payment Amount']"), ConfigSettings.WaitTime);
        paymentAmountInput.Clear();
        paymentAmountInput.SendKeys(amount);
        test.Log(AventStack.ExtentReports.Status.Info, $"Entered payment amount: {amount}.");
    }

    /// <summary>
    /// Selects payment date.
    /// </summary>
    public void SelectPaymentDate(string date)
    {
        webElementExtensions.WaitForElement(_driver, By.XPath("//input[@aria-label='Payment Date']"), ConfigSettings.WaitTime);
        paymentDateInput.Clear();
        paymentDateInput.SendKeys(date);
        test.Log(AventStack.ExtentReports.Status.Info, $"Selected payment date: {date}.");
    }

    /// <summary>
    /// Clicks 'Confirm Payment' button.
    /// </summary>
    public void ClickConfirmPayment()
    {
        webElementExtensions.WaitForElementClickable(_driver, By.XPath("//button[contains(text(),'Confirm Payment')]"), ConfigSettings.WaitTime);
        confirmPaymentBtn.Click();
        test.Log(AventStack.ExtentReports.Status.Info, "Clicked 'Confirm Payment' button.");
    }

    /// <summary>
    /// Returns true if late fee message is displayed.
    /// </summary>
    public bool IsLateFeeMessageDisplayed()
    {
        return webElementExtensions.IsElementDisplayed(_driver, By.XPath("//div[contains(@class,'late-fee-message')]"));
    }

    /// <summary>
    /// Gets the late fee message text.
    /// </summary>
    public string GetLateFeeMessageText()
    {
        return webElementExtensions.GetText(_driver, By.XPath("//div[contains(@class,'late-fee-message')]"), ConfigSettings.WaitTime);
    }
    #endregion
}
