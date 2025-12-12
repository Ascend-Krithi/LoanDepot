// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Collections.Concurrent;

namespace AutomationFramework.Core.SelfHealing
{
    /// <summary>
    /// A thread-safe repository for storing and retrieving LocatorSnapshots.
    /// This acts as the memory for the self-healing system, mapping original locators to their last known good state.
    /// </summary>
    public static class SelfHealingRepository
    {
        private static readonly ConcurrentDictionary<string, LocatorSnapshot> _repository = new ConcurrentDictionary<string, LocatorSnapshot>();

        /// <summary>
        /// Stores or updates a locator snapshot in the repository.
        /// The key is the string representation of the original 'By' locator.
        /// </summary>
        /// <param name="snapshot">The locator snapshot to store.</param>
        public static void Store(LocatorSnapshot snapshot)
        {
            var key = snapshot.OriginalLocator.ToString();
            _repository[key] = snapshot;
        }

        /// <summary>
        /// Retrieves a locator snapshot from the repository.
        /// </summary>
        /// <param name="locator">The original 'By' locator to look up.</param>
        /// <returns>The stored LocatorSnapshot, or null if not found.</returns>
        public static LocatorSnapshot Retrieve(By locator)
        {
            var key = locator.ToString();
            _repository.TryGetValue(key, out var snapshot);
            return snapshot;
        }

        /// <summary>
        /// Clears all stored snapshots from the repository.
        /// Should be called between tests to ensure a clean state.
        /// </summary>
        public static void Clear()
        {
            _repository.Clear();
        }
    }
}