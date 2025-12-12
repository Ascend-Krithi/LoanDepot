using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationFramework.Core.SelfHealing
{
    public class DomAnalyzer
    {
        private readonly IWebDriver _driver;

        public DomAnalyzer(IWebDriver driver)
        {
            _driver = driver;
        }

        public By FindAlternativeLocator(IWebElement staleElement, By originalLocator)
        {
            // This is a simplified example. A real-world implementation would be more sophisticated.
            // It might analyze attributes, text content, and proximity to other elements.

            var attributesToTry = new[] { "id", "name", "data-testid", "class", "role", "type" };
            var elementTag = staleElement.TagName;
            var elementText = staleElement.Text;

            // 1. Try finding by attributes
            foreach (var attr in attributesToTry)
            {
                var attrValue = staleElement.GetAttribute(attr);
                if (!string.IsNullOrEmpty(attrValue))
                {
                    // Sanitize class attribute
                    if (attr == "class")
                    {
                        attrValue = attrValue.Split(' ').FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
                    }

                    var cssSelector = $"{elementTag}[{attr}='{attrValue}']";
                    var newLocator = By.CssSelector(cssSelector);
                    if (IsLocatorUniqueAndValid(newLocator))
                    {
                        return newLocator;
                    }
                }
            }

            // 2. Try finding by text content (if applicable)
            if (!string.IsNullOrEmpty(elementText))
            {
                var xpath = $"//{elementTag}[normalize-space()='{elementText}']";
                var newLocator = By.XPath(xpath);
                if (IsLocatorUniqueAndValid(newLocator))
                {
                    return newLocator;
                }
            }

            // 3. Generate a more robust XPath based on multiple attributes
            var robustXPath = GenerateRobustXPath(staleElement);
            if (robustXPath != null)
            {
                var newLocator = By.XPath(robustXPath);
                if (IsLocatorUniqueAndValid(newLocator))
                {
                    return newLocator;
                }
            }

            return null; // Could not find a stable alternative
        }

        private bool IsLocatorUniqueAndValid(By locator)
        {
            try
            {
                return _driver.FindElements(locator).Count == 1;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateRobustXPath(IWebElement element)
        {
            var jsExecutor = (IJavaScriptExecutor)_driver;
            const string script = @"
                function getPathTo(element) {
                    if (element.id !== '')
                        return 'id(\'" + element.id + "\')';
                    if (element === document.body)
                        return element.tagName;

                    var ix = 0;
                    var siblings = element.parentNode.childNodes;
                    for (var i = 0; i < siblings.length; i++) {
                        var sibling = siblings[i];
                        if (sibling === element)
                            return getPathTo(element.parentNode) + '/' + element.tagName + '[' + (ix + 1) + ']';
                        if (sibling.nodeType === 1 && sibling.tagName === element.tagName)
                            ix++;
                    }
                }
                return getPathTo(arguments[0]);";

            try
            {
                var absolutePath = (string)jsExecutor.ExecuteScript(script, element);
                return "/" + absolutePath;
            }
            catch
            {
                return null;
            }
        }
    }
}