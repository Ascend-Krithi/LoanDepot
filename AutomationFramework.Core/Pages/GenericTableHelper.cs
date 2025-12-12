// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// A powerful helper class for interacting with HTML tables.
    /// </summary>
    public class GenericTableHelper
    {
        private readonly IWebElement _tableElement;
        private List<string> _headers;

        public GenericTableHelper(IWebElement tableElement)
        {
            _tableElement = tableElement;
            _headers = _tableElement.FindElements(By.TagName("th"))
                                    .Select(h => h.Text.Trim())
                                    .ToList();
        }

        /// <summary>
        /// Gets the number of rows in the table body.
        /// </summary>
        public int GetRowCount() => _tableElement.FindElements(By.CssSelector("tbody > tr")).Count;

        /// <summary>
        /// Gets the value of a cell by its row and column index.
        /// </summary>
        /// <param name="rowIndex">Zero-based row index.</param>
        /// <param name="colIndex">Zero-based column index.</param>
        /// <returns>The text content of the cell.</returns>
        public string GetCellValue(int rowIndex, int colIndex)
        {
            var row = _tableElement.FindElements(By.CssSelector("tbody > tr"))[rowIndex];
            return row.FindElements(By.TagName("td"))[colIndex].Text;
        }

        /// <summary>
        /// Gets the value of a cell by its row index and column header name.
        /// </summary>
        /// <param name="rowIndex">Zero-based row index.</param>
        /// <param name="columnName">The name of the column header.</param>
        /// <returns>The text content of the cell.</returns>
        public string GetCellValue(int rowIndex, string columnName)
        {
            int colIndex = _headers.IndexOf(columnName);
            if (colIndex == -1) throw new KeyNotFoundException($"Column '{columnName}' not found.");
            return GetCellValue(rowIndex, colIndex);
        }

        /// <summary>
        /// Finds the first row that contains a specific value in a specific column.
        /// </summary>
        /// <param name="columnName">The name of the column to search in.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The IWebElement for the matching row (tr), or null if not found.</returns>
        public IWebElement FindRowByCellValue(string columnName, string value)
        {
            int colIndex = _headers.IndexOf(columnName);
            if (colIndex == -1) throw new KeyNotFoundException($"Column '{columnName}' not found.");

            return _tableElement.FindElements(By.CssSelector("tbody > tr"))
                .FirstOrDefault(row => row.FindElements(By.TagName("td"))[colIndex].Text.Trim() == value);
        }

        /// <summary>
        /// Clicks a button or link within a specific row, identified by a cell value.
        /// </summary>
        /// <param name="lookupColumnName">The column to find the row by.</param>
        /// <param name="lookupValue">The value to identify the row.</param>
        /// <param name="targetColumnName">The column containing the button to click.</param>
        public void ClickButtonInRow(string lookupColumnName, string lookupValue, string targetColumnName)
        {
            var row = FindRowByCellValue(lookupColumnName, lookupValue);
            if (row == null) throw new NoSuchElementException($"Row with value '{lookupValue}' in column '{lookupColumnName}' not found.");

            int targetColIndex = _headers.IndexOf(targetColumnName);
            if (targetColIndex == -1) throw new KeyNotFoundException($"Target column '{targetColumnName}' not found.");

            var cell = row.FindElements(By.TagName("td"))[targetColIndex];
            // Find the first clickable element (button or a) in the cell
            var clickableElement = cell.FindElement(By.CssSelector("button, a"));
            clickableElement.Click();
        }
    }
}