using OpenQA.Selenium;
using WebAutomation.Core.Locators;

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

    public IWebElement FindElement(string key, params string[] args) =>
        _driver.FindElement(_repo.GetBy(key, args));

    public bool IsPresent(string key) =>
        _driver.FindElements(_repo.GetBy(key)).Any();

    public void Navigate(string url) =>
        _driver.Navigate().GoToUrl(url);
}