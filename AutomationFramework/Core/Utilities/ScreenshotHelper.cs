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
            var fileName = $"{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.png";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            return path;
        }
    }
}