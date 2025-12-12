using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper : BasePage
    {
        public const string TableKey = "GenericTable.Table";
        public const string RowKey = "GenericTable.Row";
        public const string CellKey = "GenericTable.Cell";

        private readonly By table = By.CssSelector("table");
        private readonly By row = By.CssSelector("tr");
        private readonly By cell = By.CssSelector("td, th");

        public GenericTableHelper(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Table => FindElement(TableKey, table);

        public IReadOnlyCollection<IWebElement> GetRows()
        {
            return Table.FindElements(row);
        }

        public IReadOnlyCollection<IWebElement> GetCells(IWebElement rowElement)
        {
            return rowElement.FindElements(cell);
        }
    }
}