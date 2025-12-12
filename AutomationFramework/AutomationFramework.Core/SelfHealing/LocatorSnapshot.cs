using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class LocatorSnapshot
    {
        public string OriginalLocator { get; set; }
        public List<string> AlternativeLocators { get; set; } = new List<string>();
        public string HealedLocator { get; set; }
        public int FailureCount { get; set; }

        public LocatorSnapshot(string originalLocator)
        {
            OriginalLocator = originalLocator;
        }
    }
}