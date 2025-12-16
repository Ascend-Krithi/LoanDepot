using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public static class DomAnalyzer
    {
        public static By Heal(string logicalKey, By originalLocator)
        {
            // Default: return original locator
            return originalLocator;
        }
    }
}