using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AutomationFramework.Core.Drivers
{
    public class WebDriverFactory
    {
        public SelfHealingWebDriver Create()
        {
            var browser = ConfigManager.TestSettings.Browser;
            IWebDriver driver;

            switch (browser.ToLowerInvariant())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    driver = new ChromeDriver(chromeOptions);
                    break;
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    // Maximizing in Firefox requires a different approach after geckodriver changes
                    // driver.Manage().Window.Maximize() is the standard way
                    driver = new FirefoxDriver(firefoxOptions);
                    break;
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--start-maximized");
                    driver = new EdgeDriver(edgeOptions);
                    break;
                default:
                    throw new ArgumentException($"Browser '{browser}' is not supported.");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2); // Small implicit wait

            return new SelfHealingWebDriver(driver);
        }
    }
}