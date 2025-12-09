using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public interface ILocatorRepository
    {
        By GetLocator(string key);
        By[] GetAlternatives(string key);
    }
}