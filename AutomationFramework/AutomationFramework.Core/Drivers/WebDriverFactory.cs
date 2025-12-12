using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Drivers
{
    public class WebDriverFactory
    {
        public IWebDriver Create(TestSettings settings)
        {
            IWebDriver driver;

            switch (settings.Browser.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    if (settings.Headless)
                    {
                        chromeOptions.AddArgument("--headless");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (settings.Headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                    }
                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (settings.Headless)
                    {
                        edgeOptions.AddArgument("headless");
                        edgeOptions.AddArgument("disable-gpu");
                    }
                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Browser not supported: {settings.Browser}");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0); // Disable implicit wait for explicit waits

            if (settings.EnableSelfHealing)
            {
                var repository = new SelfHealingRepository(settings.SelfHealingRepositoryPath);
                return new SelfHealingWebDriver(driver, repository);
            }

            return driver;
        }
    }
}