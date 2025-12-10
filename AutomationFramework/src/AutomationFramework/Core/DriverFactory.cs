using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFramework.Core
{
    public static class DriverFactory
    {
        public static IWebDriver Create()
        {
            var config = ConfigManager.Instance;

            var type = config.DriverType.Trim().ToLowerInvariant();
            return type switch
            {
                "chrome" => CreateChrome(config.Headless, config.ImplicitWaitSeconds),
                _ => throw new NotSupportedException($"Driver type '{config.DriverType}' not supported")
            };
        }

        private static IWebDriver CreateChrome(bool headless, int implicitWaitSeconds)
        {
            var options = new ChromeOptions();
            if (headless)
            {
                options.AddArgument("--headless=new");
            }
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitSeconds);
            return driver;
        }
    }
}