using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebAutomation.Core.Utilities;

public sealed class SmartWait
{
    private readonly IWebDriver _driver;
    private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(20);

    public SmartWait(IWebDriver driver)
    {
        _driver = driver;
    }

    /* ============================================================
     *  BASE WAIT
     * ============================================================ */

    public void Until(Func<IWebDriver, bool> condition)
    {
        new WebDriverWait(_driver, _defaultTimeout).Until(condition);
    }

    /* ============================================================
     *  SAFE ELEMENT WAITS (NO SeleniumExtras)
     * ============================================================ */

    public IWebElement UntilVisible(By by, int seconds = 20)
    {
        return new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d =>
            {
                var el = d.FindElement(by);
                return el.Displayed ? el : null;
            });
    }

    public IWebElement UntilClickable(By by, int seconds = 20)
    {
        return new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d =>
            {
                var el = d.FindElement(by);
                return (el.Displayed && el.Enabled) ? el : null;
            });
    }

    public bool UntilPresent(By by, int seconds = 10)
    {
        try
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
                .Until(d => d.FindElements(by).Count > 0);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void UntilNotPresent(By by, int seconds = 10)
    {
        new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d => d.FindElements(by).Count == 0);
    }

    /* ============================================================
     *  ANGULAR / CDK OVERLAY SUPPORT
     * ============================================================ */

    public void WaitForOverlay(int seconds = 10)
    {
        new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d =>
                d.FindElements(
                    By.CssSelector("cdk-overlay-pane, mat-datepicker-content")
                ).Count > 0
            );
    }

    public void WaitForOverlayToClose(int seconds = 10)
    {
        new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d =>
                d.FindElements(
                    By.CssSelector("cdk-overlay-pane, mat-datepicker-content")
                ).Count == 0
            );
    }

    public IWebElement UntilEnabled(By by, int seconds = 20)
    {
        return new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
            .Until(d =>
            {
                var el = d.FindElement(by);
                return el.Enabled ? el : null;
            });
    }
}