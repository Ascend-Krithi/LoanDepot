// NuGet Packages: Selenium.WebDriver, Selenium.WebDriver.ChromeDriver, Selenium.WebDriver.FirefoxDriver, Selenium.WebDriver.EdgeDriver
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Drivers
{
    /// <summary>
    /// Factory for creating and managing WebDriver instances.
    /// Uses Selenium Manager for automatic driver management.
    /// </summary>
    public class WebDriverFactory
    {
        /// <summary>
        /// Creates a WebDriver instance based on the settings provided in the configuration.
        /// </summary>
        /// <returns>An IWebDriver instance, potentially wrapped in a SelfHealingWebDriver.</returns>
        public IWebDriver CreateDriver()
        {
            IWebDriver driver;
            var browser = ConfigManager.Settings.Browser;
            var headless = ConfigManager.Settings.Headless;

            switch (browser.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    if (headless)
                    {
                        chromeOptions.AddArgument("--headless");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    chromeOptions.AddArgument("--start-maximized");
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                        firefoxOptions.AddArgument("--width=1920");
                        firefoxOptions.AddArgument("--height=1080");
                    }
                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (headless)
                    {
                        edgeOptions.AddArgument("--headless");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                    }
                    edgeOptions.AddArgument("--start-maximized");
                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new NotSupportedException($"Browser '{browser}' is not supported.");
            }

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigManager.Settings.DefaultTimeoutInSeconds);
            
            if (!headless && browser.ToLower() != "firefox") // Firefox maximizes via options
            {
                driver.Manage().Window.Maximize();
            }

            // Wrap the driver with the self-healing proxy if enabled
            if (ConfigManager.Settings.IsSelfHealingEnabled)
            {
                return new SelfHealingWebDriver(driver);
            }

            return driver;
        }
    }
}