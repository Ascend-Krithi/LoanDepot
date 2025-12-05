using System;
using System.IO;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class ScreenshotHelper
    {
        public static string TakeScreenshot(IWebDriver driver, string namePrefix = "screenshot")
        {
            try
            {
                var ss = ((ITakesScreenshot)driver).GetScreenshot();
                var folder = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(folder);
                var file = Path.Combine(folder, $"{namePrefix}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");
                ss.SaveAsFile(file, ScreenshotImageFormat.Png);
                return file;
            }
            catch { return string.Empty; }
        }
    }
}