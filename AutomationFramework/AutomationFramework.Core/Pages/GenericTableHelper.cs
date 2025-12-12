using OpenQA.Selenium;
using System.Data;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper
    {
        private readonly IWebElement _tableElement;

        public GenericTableHelper(IWebElement tableElement)
        {
            _tableElement = tableElement;
        }

        public IWebElement FindRowByText(string searchText, int searchColumnIndex = 0)
        {
            var rows = _tableElement.FindElements(By.TagName("tr"));
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count > searchColumnIndex && cells[searchColumnIndex].Text.Contains(searchText))
                {
                    return row;
                }
            }
            throw new NoSuchElementException($"Row containing text '{searchText}' not found in table.");
        }

        public string GetCellValue(int rowIndex, int colIndex)
        {
            var row = _tableElement.FindElements(By.TagName("tr"))[rowIndex];
            return row.FindElements(By.TagName("td"))[colIndex].Text;
        }

        public DataTable GetTableData()
        {
            var dataTable = new DataTable();
            var headerCells = _tableElement.FindElements(By.TagName("th"));
            foreach (var header in headerCells)
            {
                dataTable.Columns.Add(header.Text);
            }

            var rows = _tableElement.FindElements(By.CssSelector("tbody tr"));
            foreach (var row in rows)
            {
                var dataRow = dataTable.NewRow();
                var cells = row.FindElements(By.TagName("td"));
                for (int i = 0; i < cells.Count; i++)
                {
                    dataRow[i] = cells[i].Text;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }
}