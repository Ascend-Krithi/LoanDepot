using TechTalk.SpecFlow;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using OpenQA.Selenium;
using System;
using System.IO;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private static SelfHealingWebDriver _driver;

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            _driver = WebDriverFactory.CreateDriver();
            ScenarioContext.Current["WebDriver"] = _driver;
        }

        [AfterStep]
        public void AfterStep()
        {
            var scenarioContext = ScenarioContext.Current;
            if (scenarioContext.TestError != null)
            {
                var driver = ScenarioContext.Current["WebDriver"] as SelfHealingWebDriver;
                if (driver != null)
                {
                    var screenshot = ((ITakesScreenshot)driver.InnerDriver).GetScreenshot();
                    var fileName = $"screenshot_{Guid.NewGuid()}.png";
                    var screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(screenshotsDir);
                    screenshot.SaveAsFile(Path.Combine(screenshotsDir, fileName), ScreenshotImageFormat.Png);
                }
            }
        }

        [AfterScenario(Order = 100)]
        public void AfterScenario()
        {
            var driver = ScenarioContext.Current["WebDriver"] as SelfHealingWebDriver;
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}