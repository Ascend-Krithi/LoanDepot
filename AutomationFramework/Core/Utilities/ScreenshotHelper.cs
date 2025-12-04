using OpenQA.Selenium;
using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshot(IWebDriver driver, string scenarioName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var fileName = $"{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, fileName);
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            return filePath;
        }
    }
}