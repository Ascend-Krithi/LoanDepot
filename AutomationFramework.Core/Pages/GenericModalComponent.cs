// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// A helper class to interact with generic modal dialogs (e.g., Bootstrap modals).
    /// It is instantiated with the root element of the modal.
    /// </summary>
    public class GenericModalComponent
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelper _wait;
        private readonly IWebElement _modalRootElement;

        // Common relative locators for modal parts
        private readonly By _titleLocator = By.ClassName("modal-title");
        private readonly By _bodyLocator = By.ClassName("modal-body");
        private readonly By _footerLocator = By.ClassName("modal-footer");

        public GenericModalComponent(IWebDriver driver, By modalRootLocator)
        {
            _driver = driver;
            _wait = new WaitHelper(driver);
            _modalRootElement = _wait.WaitForElementVisible(modalRootLocator);
        }

        /// <summary>
        /// Gets the title text of the modal.
        /// </summary>
        public string GetTitle()
        {
            return _modalRootElement.FindElement(_titleLocator).Text;
        }

        /// <summary>
        /// Gets the body text of the modal.
        /// </summary>
        public string GetBodyText()
        {
            return _modalRootElement.FindElement(_bodyLocator).Text;
        }

        /// <summary>
        /// Clicks a button within the modal's footer based on its visible text.
        /// </summary>
        /// <param name="buttonText">The text of the button to click (case-insensitive).</param>
        public void ClickFooterButton(string buttonText)
        {
            var footer = _modalRootElement.FindElement(_footerLocator);
            var button = footer.FindElements(By.TagName("button"))
                               .FirstOrDefault(b => b.Text.Trim().Equals(buttonText, System.StringComparison.OrdinalIgnoreCase));

            if (button == null)
            {
                throw new NoSuchElementException($"Button with text '{buttonText}' not found in the modal footer.");
            }
            button.Click();
        }

        /// <summary>
        /// Clicks the close button (typically an 'X' in the header).
        /// </summary>
        public void ClickCloseButton()
        {
            var closeButton = _modalRootElement.FindElement(By.ClassName("close")); // Common class for close buttons
            closeButton.Click();
        }
    }
}