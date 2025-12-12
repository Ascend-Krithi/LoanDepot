// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    /// <summary>
    /// Represents a snapshot of a web element's attributes at the time it was successfully located.
    /// This data is used by the self-healing mechanism to find the element if its original locator fails.
    /// </summary>
    public class LocatorSnapshot
    {
        /// <summary>
        /// The original locator used to find the element.
        /// </summary>
        public By OriginalLocator { get; }

        /// <summary>
        /// The tag name of the element (e.g., 'input', 'button').
        /// </summary>
        public string TagName { get; }

        /// <summary>
        /// The inner text of the element.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// A dictionary of the element's attributes (e.g., id, name, class, href).
        /// </summary>
        public Dictionary<string, string> Attributes { get; }

        public LocatorSnapshot(IWebElement element, By originalLocator)
        {
            OriginalLocator = originalLocator;
            TagName = element.TagName;
            Text = element.Text;
            Attributes = new Dictionary<string, string>();

            // Capture common and important attributes
            var attributeNames = new[] { "id", "name", "class", "type", "value", "href", "placeholder", "aria-label", "data-testid" };
            foreach (var attrName in attributeNames)
            {
                var attrValue = element.GetAttribute(attrName);
                if (!string.IsNullOrEmpty(attrValue))
                {
                    Attributes[attrName] = attrValue;
                }
            }
        }
    }
}