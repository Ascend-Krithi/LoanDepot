using System.Collections.Concurrent;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingRepository
    {
        private readonly ConcurrentDictionary<string, LocatorSnapshot> _storage = new();

        public void AddOrUpdate(string logicalKey, LocatorSnapshot snapshot)
        {
            _storage.AddOrUpdate(logicalKey, snapshot, (k, v) => snapshot);
        }

        public LocatorSnapshot Get(string logicalKey)
        {
            _storage.TryGetValue(logicalKey, out var snapshot);
            return snapshot;
        }
    }
}