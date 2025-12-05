using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LD_AutomationFramework
{
    [TestClass]
    public class Browser : BasePage
    {
        public enum BrowserType
        {
            Chrome,
            Edge,
            Firefox
        }

        public IWebDriver WebDriver(string type, out int browserPID)
        {
            IWebDriver driver = null;

            switch((BrowserType)Enum.Parse(typeof(BrowserType), type, true))
            {
                case BrowserType.Chrome:
                    driver = ChromeDriver(out browserPID);
                    break;
                case BrowserType.Edge:
                    driver = EdgeDriver(out browserPID);
                    break;
                case BrowserType.Firefox:
                    driver = FirefoxDriver(out browserPID);
                    break;
                default:
                    driver = ChromeDriver(out browserPID);
                    break;
            }
            return driver;
        }

        ///<summary>
        ///Method to create Chrome Webdriver instance
        ///</summary>
        public IWebDriver ChromeDriver(out int browserPID)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            chromeOptions.AddArgument("--incognito");
            chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            chromeOptions.AddArgument("--start-maximized");
            ChromeDriverService chromeService = ChromeDriverService.CreateDefaultService();
            var _driver = new ChromeDriver(chromeService, chromeOptions);
            browserPID = chromeService.ProcessId;
            return _driver;
        }

        ///<summary>
        ///Method to create Edge Webdriver instance
        ///</summary>
        public IWebDriver EdgeDriver(out int browserPID)
        {
            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument("--start-maximized");            
            EdgeDriverService edgeService = EdgeDriverService.CreateDefaultService();
            var _driver = new EdgeDriver(edgeService, edgeOptions);
            browserPID = edgeService.ProcessId;
            return _driver;
        }

        ///<summary>
        ///Method to create Firefox Webdriver instance
        ///</summary>
        public IWebDriver FirefoxDriver(out int browserPID)
        {
            var _driver = new FirefoxDriver();
            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();
            browserPID = firefoxService.ProcessId;
            return _driver;
        }
    }
}
