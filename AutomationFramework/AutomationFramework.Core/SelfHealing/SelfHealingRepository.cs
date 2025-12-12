using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingRepository
    {
        private readonly string _filePath;
        private readonly ConcurrentDictionary<string, LocatorSnapshot> _repository;

        public SelfHealingRepository(string filePath)
        {
            _filePath = filePath;
            _repository = LoadRepository();
        }

        private ConcurrentDictionary<string, LocatorSnapshot> LoadRepository()
        {
            if (!File.Exists(_filePath))
            {
                return new ConcurrentDictionary<string, LocatorSnapshot>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<ConcurrentDictionary<string, LocatorSnapshot>>(json)
                   ?? new ConcurrentDictionary<string, LocatorSnapshot>();
        }

        public void SaveRepository()
        {
            var json = JsonSerializer.Serialize(_repository, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public LocatorSnapshot Get(By locator)
        {
            _repository.TryGetValue(locator.ToString(), out var snapshot);
            return snapshot;
        }

        public void Update(By originalLocator, By newHealedLocator)
        {
            var key = originalLocator.ToString();
            var snapshot = _repository.GetOrAdd(key, new LocatorSnapshot(key));
            snapshot.HealedLocator = newHealedLocator.ToString();
            snapshot.FailureCount = 0; // Reset failure count after successful healing
        }

        public void RecordFailure(By locator)
        {
            var key = locator.ToString();
            var snapshot = _repository.GetOrAdd(key, new LocatorSnapshot(key));
            snapshot.FailureCount++;
        }
    }
}