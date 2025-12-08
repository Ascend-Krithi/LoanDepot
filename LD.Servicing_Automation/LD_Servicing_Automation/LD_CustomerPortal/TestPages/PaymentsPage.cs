using OpenQA.Selenium;
using AventStack.ExtentReports;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

/// <summary>
/// PaymentsPage class for Customer Portal - Implements One-Time Payment logic as per KB (see 'PaymentsPage' skeleton in KB)
/// Test Case Coverage: CUSTP-2005 TS-001 TC-001 to CUSTP-2005 TS-009 TC-001
/// </summary>
public class PaymentsPage
{
    private IWebDriver _driver;
    private ExtentTest test;
    private WebElementExtensionsPage webElementExtensions;

    /// <summary>
    /// Constructor initializes driver, test, and webElementExtensions
    /// </summary>
    public PaymentsPage(IWebDriver driver, ExtentTest test)
    {
        this._driver = driver;
        this.test = test;
        webElementExtensions = new WebElementExtensionsPage(driver, test);
        PageFactory.InitElements(driver, this);
    }

    #region Locators
    // Locators mapped from HTML analysis and KB
    // NOTE: No interactive tags found in Servicing _ loanDepot_payment.html, so plausible XPaths are generated

    /// <summary>
    /// Locator for HELOC account dropdown (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By helocAccountDropdownLocBy = By.XPath("//select[@id='helocAccountDropdown']"); // [Generated: Servicing _ loanDepot_payment.html]

    /// <summary>
    /// Locator for One-Time Payment button (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By oneTimePaymentButtonLocBy = By.XPath("//button[contains(text(),'One-Time Payment')]"); // [Generated: Servicing _ loanDepot_payment.html]

    /// <summary>
    /// Locator for Payment Amount input (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By paymentAmountInputLocBy = By.XPath("//input[@id='paymentAmount']"); // [Generated: Servicing _ loanDepot_payment.html]

    /// <summary>
    /// Locator for Payment Date picker (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By paymentDatePickerLocBy = By.XPath("//input[@id='paymentDate']"); // [Generated: Servicing _ loanDepot_payment.html]

    /// <summary>
    /// Locator for Submit Payment button (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By submitPaymentButtonLocBy = By.XPath("//button[contains(text(),'Submit Payment')]"); // [Generated: Servicing _ loanDepot_payment.html]

    /// <summary>
    /// Locator for Late Fee message (Servicing _ loanDepot_payment.html)
    /// </summary>
    public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message')]"); // [Generated: Servicing _ loanDepot_payment.html]
    #endregion

    #region Methods
    /// <summary>
    /// Selects the HELOC account by account number
    /// </summary>
    /// <param name="accountNumber">HELOC Account Number</param>
    public void SelectHelocAccount(string accountNumber)
    {
        test.Log(AventStack.ExtentReports.Status.Info, $"Selecting HELOC account: {accountNumber}");
        webElementExtensions.WaitForElement(_driver, helocAccountDropdownLocBy, ConfigSettings.WaitTime);
        var dropdown = _driver.FindElement(helocAccountDropdownLocBy);
        dropdown.Click();
        dropdown.SendKeys(accountNumber);
    }

    /// <summary>
    /// Clicks the One-Time Payment button
    /// </summary>
    public void ClickOneTimePayment()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Clicking One-Time Payment button");
        webElementExtensions.WaitForElementClickable(_driver, oneTimePaymentButtonLocBy, ConfigSettings.WaitTime);
        webElementExtensions.Click(_driver, oneTimePaymentButtonLocBy, ConfigSettings.WaitTime);
    }

    /// <summary>
    /// Enters the payment amount
    /// </summary>
    /// <param name="amount">Payment Amount</param>
    public void EnterPaymentAmount(string amount)
    {
        test.Log(AventStack.ExtentReports.Status.Info, $"Entering payment amount: {amount}");
        webElementExtensions.WaitForElement(_driver, paymentAmountInputLocBy, ConfigSettings.WaitTime);
        webElementExtensions.SendKeys(_driver, paymentAmountInputLocBy, amount, ConfigSettings.WaitTime);
    }

    /// <summary>
    /// Selects the payment date
    /// </summary>
    /// <param name="date">Payment Date (MM/dd/yyyy)</param>
    public void SelectPaymentDate(string date)
    {
        test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {date}");
        webElementExtensions.WaitForElement(_driver, paymentDatePickerLocBy, ConfigSettings.WaitTime);
        webElementExtensions.SendKeys(_driver, paymentDatePickerLocBy, date, ConfigSettings.WaitTime);
    }

    /// <summary>
    /// Submits the payment
    /// </summary>
    public void SubmitPayment()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Submitting payment");
        webElementExtensions.WaitForElementClickable(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
        webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
    }

    /// <summary>
    /// Gets the late fee message text
    /// </summary>
    /// <returns>Late fee message</returns>
    public string GetLateFeeMessage()
    {
        test.Log(AventStack.ExtentReports.Status.Info, "Getting late fee message");
        webElementExtensions.WaitForElement(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
        return webElementExtensions.GetText(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
    }

    /// <summary>
    /// Checks if late fee message is displayed
    /// </summary>
    /// <returns>True if displayed, else false</returns>
    public bool IsLateFeeMessageDisplayed()
    {
        return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
    }
    #endregion
}
