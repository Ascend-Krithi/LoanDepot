using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver()
        {
            var browser = ConfigManager.Settings.Browser?.ToLowerInvariant() ?? "chrome";
            var headless = ConfigManager.Settings.Headless;

            switch (browser)
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    if (headless) chromeOptions.AddArgument("--headless=new");
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--lang=" + ConfigManager.Settings.Language);
                    return new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions);

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (headless) firefoxOptions.AddArgument("--headless");
                    firefoxOptions.AddArgument("--width=1920");
                    firefoxOptions.AddArgument("--height=1080");
                    firefoxOptions.SetPreference("intl.accept_languages", ConfigManager.Settings.Language);
                    return new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), firefoxOptions);

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (headless) edgeOptions.AddArgument("headless");
                    edgeOptions.AddArgument("start-maximized");
                    edgeOptions.AddArgument("--lang=" + ConfigManager.Settings.Language);
                    return new EdgeDriver(EdgeDriverService.CreateDefaultService(), edgeOptions);

                default:
                    throw new NotSupportedException($"Browser not supported: {browser}");
            }
        }
    }
}