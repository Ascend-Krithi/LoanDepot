using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using System.Collections.ObjectModel;

namespace WebAutomation.Core.SelfHealing;

public sealed class SelfHealingWebDriver
{
    private readonly IWebDriver _driver;
    private readonly LocatorRepository _repo;

    public SelfHealingWebDriver(IWebDriver driver, LocatorRepository repo)
    {
        _driver = driver;
        _repo = repo;
    }

    public IWebElement FindElement(string key, params string[] args)
    {
        // Basic implementation: find by the key provided.
        // A true self-healing implementation would involve trying alternative locators
        // from the repository if the primary one fails.
        try
        {
            return _driver.FindElement(_repo.GetBy(key, args));
        }
        catch (NoSuchElementException)
        {
            // TODO: Implement healing logic.
            // 1. Look for alternative locators for the given 'key'.
            // 2. If an alternative works, log the success and maybe suggest updating the primary locator.
            // 3. If all fail, re-throw the exception.
            Console.WriteLine($"Self-Healing: Element '{key}' not found with primary locator. Healing not yet implemented.");
            throw;
        }
    }

    public ReadOnlyCollection<IWebElement> FindElements(string key, params string[] args)
    {
        return _driver.FindElements(_repo.GetBy(key, args));
    }

    public bool IsPresent(string key, params string[] args)
    {
        return _driver.FindElements(_repo.GetBy(key, args)).Any();
    }

    public void Navigate(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
}