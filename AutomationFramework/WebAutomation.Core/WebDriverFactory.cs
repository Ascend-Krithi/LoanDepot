using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Core.Drivers;

public static class WebDriverFactory
{
    public static IWebDriver Create()
    {
        var options = new ChromeOptions();
        if (ConfigManager.Settings.Headless)
            options.AddArgument("--headless=new");

        return new ChromeDriver(options);
    }
}