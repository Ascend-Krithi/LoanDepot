using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper : BasePage
    {
        private readonly IWebElement _tableElement;

        public GenericTableHelper(SelfHealingWebDriver driver, By tableLocator) : base(driver)
        {
            _tableElement = FindElement("GenericTable.TableElement", tableLocator);
        }

        public ReadOnlyCollection<IWebElement> GetRows()
        {
            return _tableElement.FindElements(By.TagName("tr"));
        }

        public IWebElement? GetCell(int rowIndex, int colIndex)
        {
            var rows = GetRows();
            if (rowIndex >= rows.Count) return null;

            var cells = rows[rowIndex].FindElements(By.TagName("td"));
            return colIndex < cells.Count ? cells[colIndex] : null;
        }
    }
}