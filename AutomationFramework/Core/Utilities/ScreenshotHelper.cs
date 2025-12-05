using System;
using System.IO;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver, string scenarioName)
        {
            try
            {
                var ss = ((ITakesScreenshot)driver).GetScreenshot();
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                Directory.CreateDirectory(dir);
                var path = Path.Combine(dir, $DateTime.UtcNow:yyyyMMddHHmmss");
                ss.SaveAsFile(path);
                return path;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
