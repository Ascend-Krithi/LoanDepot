using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper : BasePage
    {
        public const string TableKey = "GenericTable.Table";
        public const string RowKey = "GenericTable.Row";
        public const string CellKey = "GenericTable.Cell";

        private readonly By _table = By.CssSelector("table");
        private readonly By _row = By.CssSelector("tr");
        private readonly By _cell = By.CssSelector("td,th");

        public GenericTableHelper(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Table => FindElement(TableKey, _table);

        public IList<IWebElement> GetRows()
        {
            return Table.FindElements(_row);
        }

        public IList<IWebElement> GetCells(IWebElement row)
        {
            return row.FindElements(_cell);
        }
    }
}