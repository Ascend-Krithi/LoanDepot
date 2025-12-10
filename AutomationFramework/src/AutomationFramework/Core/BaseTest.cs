using AutomationFramework.Data;
using AutomationFramework.Pages;
using AutomationFramework.Utils;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationFramework.Core
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected IWebDriver? Driver;
        protected IDataProvider? DataProvider;
        protected ScreenFlow? Flow;

        // Pages
        protected LoginPage? LoginPage;
        protected DashboardPage? DashboardPage;
        protected ServicingListingPage? ServicingListingPage;
        protected ServicingCreateEditPage? ServicingCreateEditPage;

        [SetUp]
        public void SetUp()
        {
            Logger.Info("Initializing WebDriver...");
            Driver = DriverFactory.Create();

            Logger.Info("Initializing Data Provider...");
            DataProvider = new ExcelDataProvider(ConfigManager.Instance.TestDataPath);

            Logger.Info("Loading Screen Flow...");
            Flow = ScreenFlowLoader.Load(ConfigManager.Instance.ScreenFlowPath);

            var baseUrl = ConfigManager.Instance.BaseUrl;
            Logger.Info($"Navigating to Base URL: {baseUrl}");
            Driver.Navigate().GoToUrl(baseUrl);

            // Instantiate page objects
            LoginPage = new LoginPage(Driver);
            DashboardPage = new DashboardPage(Driver);
            ServicingListingPage = new ServicingListingPage(Driver);
            ServicingCreateEditPage = new ServicingCreateEditPage(Driver);
        }

        [TearDown]
        public void TearDown()
        {
            var close = ConfigManager.Instance.CloseBrowserAfterTest;
            if (Driver != null && close)
            {
                Logger.Info("Closing WebDriver...");
                try
                {
                    Driver.Quit();
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Error while closing driver: {ex.Message}");
                }
                Driver = null;
            }
        }
    }
}