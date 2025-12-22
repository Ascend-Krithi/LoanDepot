using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Core.Utilities;

public sealed class SmartWait
{
    private readonly IWebDriver _driver;
    private readonly TimeSpan _defaultTimeout;

    public SmartWait(IWebDriver driver)
    {
        _driver = driver;
        _defaultTimeout = TimeSpan.FromSeconds(ConfigManager.Settings.DefaultTimeoutSeconds);
    }

    /* ============================================================
     *  BASE WAIT
     * ============================================================ */

    public void Until(Func<IWebDriver, bool> condition, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        new WebDriverWait(_driver, timeout).Until(condition);
    }

    /* ============================================================
     *  SAFE ELEMENT WAITS
     * ============================================================ */

    public IWebElement UntilVisible(By by, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        return new WebDriverWait(_driver, timeout)
            .Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return el.Displayed ? el : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            }) ?? throw new WebDriverTimeoutException($"Element located by {by} was not visible within {timeout.TotalSeconds} seconds.");
    }

    public IWebElement UntilClickable(By by, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        return new WebDriverWait(_driver, timeout)
            .Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return (el.Displayed && el.Enabled) ? el : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            }) ?? throw new WebDriverTimeoutException($"Element located by {by} was not clickable within {timeout.TotalSeconds} seconds.");
    }

    public bool UntilPresent(By by, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        try
        {
            new WebDriverWait(_driver, timeout)
                .Until(d => d.FindElements(by).Count > 0);
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public void UntilNotPresent(By by, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        new WebDriverWait(_driver, timeout)
            .Until(d => d.FindElements(by).Count == 0);
    }

    /* ============================================================
     *  ANGULAR / CDK OVERLAY SUPPORT
     * ============================================================ */

    public void WaitForOverlay(int seconds = 10)
    {
        var overlayLocator = By.CssSelector("div.cdk-overlay-container, mat-datepicker-content");
        Until(d => d.FindElements(overlayLocator).Any(e => e.Displayed), seconds);
    }

    public void WaitForOverlayToClose(int seconds = 10)
    {
        var overlayLocator = By.CssSelector("div.cdk-overlay-container, mat-datepicker-content");
        Until(d => !d.FindElements(overlayLocator).Any(e => e.Displayed), seconds);
    }

    public IWebElement UntilEnabled(By by, int? seconds = null)
    {
        var timeout = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : _defaultTimeout;
        return new WebDriverWait(_driver, timeout)
            .Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return el.Enabled ? el : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            }) ?? throw new WebDriverTimeoutException($"Element located by {by} was not enabled within {timeout.TotalSeconds} seconds.");
    }
}