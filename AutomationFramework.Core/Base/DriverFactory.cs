using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFramework.Core.Base
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver(bool headless = false)
        {
            var options = new ChromeOptions();
            if (headless)
                options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            return new ChromeDriver(options);
        }
    }
}