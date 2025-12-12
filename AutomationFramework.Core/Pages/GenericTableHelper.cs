// AutomationFramework.Core/Pages/GenericTableHelper.cs
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper : BasePage
    {
        private readonly By _tableLocator;
        private readonly string _tableKey;

        public GenericTableHelper(SelfHealingWebDriver driver, By tableLocator, string tableKey) : base(driver)
        {
            _tableLocator = tableLocator;
            _tableKey = tableKey;
        }

        private IWebElement TableElement => FindElement(_tableKey, _tableLocator);

        public IReadOnlyCollection<IWebElement> GetRows()
        {
            return TableElement.FindElements(By.TagName("tr"));
        }

        public IWebElement? GetRow(int rowIndex)
        {
            return GetRows().ElementAtOrDefault(rowIndex);
        }

        public IWebElement? GetCell(int rowIndex, int colIndex)
        {
            var row = GetRow(rowIndex);
            return row?.FindElements(By.TagName("td")).ElementAtOrDefault(colIndex);
        }
    }
}