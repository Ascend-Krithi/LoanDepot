using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public class LocatorSnapshot
    {
        public string LogicalKey { get; }
        public By Locator { get; set; }

        public LocatorSnapshot(string logicalKey, By locator)
        {
            LogicalKey = logicalKey;
            Locator = locator;
        }
    }
}