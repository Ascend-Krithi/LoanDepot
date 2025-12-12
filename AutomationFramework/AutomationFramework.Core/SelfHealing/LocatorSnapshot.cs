using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public class LocatorSnapshot
    {
        public string ElementName { get; set; }
        public string LocatorType { get; set; }
        public string LocatorValue { get; set; }
        public string OuterHtml { get; set; }
        public string InnerText { get; set; }

        public LocatorSnapshot() { }

        public LocatorSnapshot(string elementName, By by, string outerHtml, string innerText)
        {
            ElementName = elementName;
            LocatorType = by.GetType().Name;
            LocatorValue = by.ToString();
            OuterHtml = outerHtml;
            InnerText = innerText;
        }
    }
}