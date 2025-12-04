using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutomationFramework.Core.Locators
{
    public class LocatorRepository
    {
        private readonly Dictionary<string, List<By>> _locatorMap = new();

        public LocatorRepository()
        {
            LoadLocators();
        }

        private void LoadLocators()
        {
            var locatorTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in locatorTypes)
            {
                if (type.Namespace != null && type.Namespace.Contains("AutomationFramework.Core.Locators") && type.Name.EndsWith("Locators"))
                {
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (field.FieldType == typeof(By))
                        {
                            string key = field.Name;
                            var by = (By)field.GetValue(null);
                            if (!_locatorMap.ContainsKey(key))
                                _locatorMap[key] = new List<By>();
                            _locatorMap[key].Add(by);
                        }
                        else if (field.FieldType == typeof(By[]))
                        {
                            var byArr = (By[])field.GetValue(null);
                            foreach (var by in byArr)
                            {
                                if (!_locatorMap.ContainsKey(field.Name))
                                    _locatorMap[field.Name] = new List<By>();
                                _locatorMap[field.Name].Add(by);
                            }
                        }
                    }
                }
            }
        }

        public By[] GetLocators(string key)
        {
            if (_locatorMap.ContainsKey(key))
                return _locatorMap[key].ToArray();
            throw new KeyNotFoundException($"Locator key '{key}' not found in repository.");
        }
    }
}