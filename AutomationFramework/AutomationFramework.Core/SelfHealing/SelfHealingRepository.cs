using OpenQA.Selenium;
using System.Collections.Concurrent;

namespace AutomationFramework.Core.SelfHealing
{
    public static class SelfHealingRepository
    {
        private static readonly ConcurrentDictionary<string, LocatorSnapshot> s_locators = new();

        public static void StoreLocator(string logicalKey, By locator)
        {
            var snapshot = new LocatorSnapshot(logicalKey, locator);
            s_locators.AddOrUpdate(logicalKey, snapshot, (key, old) => snapshot);
        }

        public static By GetLocator(string logicalKey)
        {
            if (s_locators.TryGetValue(logicalKey, out var snapshot))
            {
                return snapshot.Locator;
            }
            throw new KeyNotFoundException($"Logical key '{logicalKey}' not found in repository.");
        }

        public static void UpdateLocator(string logicalKey, By newLocator)
        {
            if (s_locators.TryGetValue(logicalKey, out var snapshot))
            {
                snapshot.Locator = newLocator;
            }
        }
    }
}