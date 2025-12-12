using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public class DomAnalyzer
    {
        public By Heal(By originalLocator)
        {
            // For now, just return the original locator.
            return originalLocator;
        }
    }
}