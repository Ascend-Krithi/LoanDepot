using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

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

        public IList<IWebElement> GetRows()
        {
            return Table.FindElements(row);
        }

        public IList<IWebElement> GetCells(IWebElement rowElement)
        {
            return rowElement.FindElements(cell);
        }

        public IWebElement GetCell(int rowIndex, int colIndex)
        {
            var rows = GetRows();
            if (rowIndex < 0 || rowIndex >= rows.Count)
                return null;
            var cells = GetCells(rows[rowIndex]);
            if (colIndex < 0 || colIndex >= cells.Count)
                return null;
            return cells[colIndex];
        }
    }
}