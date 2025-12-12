using OpenQA.Selenium;
using System;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver, IJavaScriptExecutor, ITakesScreenshot
    {
        private readonly IWebDriver _driver;
        private readonly SelfHealingRepository _repository;

        public SelfHealingWebDriver(IWebDriver driver, SelfHealingRepository repository)
        {
            _driver = driver;
            _repository = repository;
        }

        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public void Quit() => _driver.Quit();
        public void Navigate() => _driver.Navigate();
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();

        public IWebElement FindElement(By by)
        {
            try
            {
                var element = _driver.FindElement(by);
                SaveSnapshot(by, element);
                return element;
            }
            catch (NoSuchElementException)
            {
                var key = by.ToString();
                var snapshot = _repository.Get(key);
                if (snapshot != null)
                {
                    var healedBy = DomAnalyzer.FindSimilarElement(_driver, snapshot);
                    if (healedBy != null)
                    {
                        Logger.Log($"Self-healing: Found alternative locator for {key}: {healedBy}");
                        var element = _driver.FindElement(healedBy);
                        SaveSnapshot(healedBy, element);
                        return element;
                    }
                }
                throw;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            try
            {
                var elements = _driver.FindElements(by);
                if (elements.Count > 0)
                {
                    SaveSnapshot(by, elements[0]);
                }
                return elements;
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        private void SaveSnapshot(By by, IWebElement element)
        {
            try
            {
                var outerHtml = element.GetAttribute("outerHTML");
                var innerText = element.Text;
                var snapshot = new LocatorSnapshot(by.ToString(), by, outerHtml, innerText);
                _repository.AddOrUpdate(by.ToString(), snapshot);
            }
            catch { }
        }

        // IJavaScriptExecutor
        public object ExecuteScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(script, args);
        }

        // ITakesScreenshot
        public Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot)_driver).GetScreenshot();
        }
    }
}