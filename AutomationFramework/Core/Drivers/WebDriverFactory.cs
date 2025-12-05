using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--remote-allow-origins=*");
            return new ChromeDriver(options);
        }
    }
}
