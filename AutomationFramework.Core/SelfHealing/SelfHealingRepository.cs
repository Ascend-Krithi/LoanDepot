// AutomationFramework.Core/SelfHealing/SelfHealingRepository.cs
using System.Collections.Concurrent;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingRepository
    {
        private readonly ConcurrentDictionary<string, LocatorSnapshot> _repository = new ConcurrentDictionary<string, LocatorSnapshot>();

        public void AddOrUpdate(string logicalKey, LocatorSnapshot snapshot)
        {
            _repository.AddOrUpdate(logicalKey, snapshot, (key, existingVal) => snapshot);
        }

        public LocatorSnapshot? Get(string logicalKey)
        {
            _repository.TryGetValue(logicalKey, out var snapshot);
            return snapshot;
        }
    }
}