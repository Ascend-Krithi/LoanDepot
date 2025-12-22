using OpenQA.Selenium;
using System.Text.Json;

namespace WebAutomation.Core.Locators
{
    public sealed class LocatorRepository
    {
        private readonly Dictionary<string, Locator> _map;

        public LocatorRepository(string relativePath)
        {
            var fullPath = Path.Combine(
                AppContext.BaseDirectory,
                relativePath
            );

            if (!File.Exists(fullPath))
                throw new FileNotFoundException(
                    $"Locators file not found at: {fullPath}");

            var json = File.ReadAllText(fullPath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _map = JsonSerializer.Deserialize<Dictionary<string, Locator>>(json, options)
                   ?? throw new InvalidOperationException(
                       "Failed to deserialize Locators.json");
        }

        public By GetBy(string key, params string[] args)
        {
            if (!_map.TryGetValue(key, out var loc))
                throw new KeyNotFoundException($"Locator not found: {key}");

            if (string.IsNullOrWhiteSpace(loc.Value))
                throw new InvalidOperationException(
                    $"Locator value is null/empty for key: {key}");

            var value = loc.Value;

            if (args?.Length > 0 && value.Contains('{'))
            {
                value = string.Format(value, args);
            }

            return loc.Type.ToLowerInvariant() switch
            {
                "id" => By.Id(value),
                "css" or "cssselector" => By.CssSelector(value),
                "xpath" => By.XPath(value),
                "name" => By.Name(value),
                "classname" => By.ClassName(value),
                "tagname" => By.TagName(value),
                "linktext" => By.LinkText(value),
                "partiallinktext" => By.PartialLinkText(value),
                _ => throw new NotSupportedException(
                    $"Unsupported locator type: {loc.Type}")
            };
        }

        private record Locator(string Type, string Value);
    }
}