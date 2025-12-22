using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Core.Drivers;

public static class WebDriverFactory
{
    public static IWebDriver Create()
    {
        var browser = ConfigManager.Settings.Browser.ToLowerInvariant();
        var headless = ConfigManager.Settings.Headless;

        return browser switch
        {
            "chrome" => CreateChromeDriver(headless),
            "firefox" => CreateFirefoxDriver(headless),
            "edge" => CreateEdgeDriver(headless),
            _ => throw new NotSupportedException($"Browser '{browser}' is not supported.")
        };
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();
        if (headless)
        {
            options.AddArgument("--headless=new");
        }
        options.AddArgument("--start-maximized");
        return new ChromeDriver(options);
    }

    private static IWebDriver CreateFirefoxDriver(bool headless)
    {
        var options = new FirefoxOptions();
        if (headless)
        {
            options.AddArgument("--headless");
        }
        return new FirefoxDriver(options);
    }

    private static IWebDriver CreateEdgeDriver(bool headless)
    {
        var options = new EdgeOptions();
        if (headless)
        {
            options.AddArgument("--headless=new");
        }
        options.AddArgument("--start-maximized");
        return new EdgeDriver(options);
    }
}