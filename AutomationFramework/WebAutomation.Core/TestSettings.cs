using OpenQA.Selenium;

namespace WebAutomation.Core.Drivers;

public sealed class DriverManager : IDisposable
{
    public IWebDriver Driver { get; }

    public DriverManager()
    {
        Driver = WebDriverFactory.Create();
    }

    public void Dispose()
    {
        try { Driver.Quit(); } catch { }
        Driver.Dispose();
    }
}