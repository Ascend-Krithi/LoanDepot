using System.Collections.Concurrent;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingRepository
    {
        private readonly ConcurrentDictionary<string, LocatorSnapshot> _snapshots = new();

        public void AddOrUpdate(string logicalKey, LocatorSnapshot snapshot)
        {
            _snapshots.AddOrUpdate(logicalKey, snapshot, (k, v) => snapshot);
        }

        public LocatorSnapshot Get(string logicalKey)
        {
            _snapshots.TryGetValue(logicalKey, out var snapshot);
            return snapshot;
        }
    }
}