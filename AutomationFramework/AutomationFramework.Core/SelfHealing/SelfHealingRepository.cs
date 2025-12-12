using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingRepository
    {
        private readonly ConcurrentDictionary<string, LocatorSnapshot> _repository = new();

        public void AddOrUpdate(string key, LocatorSnapshot snapshot)
        {
            _repository.AddOrUpdate(key, snapshot, (k, v) => snapshot);
        }

        public LocatorSnapshot Get(string key)
        {
            _repository.TryGetValue(key, out var snapshot);
            return snapshot;
        }

        public IEnumerable<LocatorSnapshot> GetAll()
        {
            return _repository.Values;
        }
    }
}