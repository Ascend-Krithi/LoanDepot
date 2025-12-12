using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent
    {
        private readonly IWebDriver _driver;
        private readonly By _modalRoot;

        public GenericModalComponent(IWebDriver driver, By modalRoot)
        {
            _driver = driver;
            _modalRoot = modalRoot;
        }

        public bool IsVisible()
        {
            try
            {
                return _driver.FindElement(_modalRoot).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            var closeBtn = _driver.FindElement(By.CssSelector($"{_modalRoot.ToString()} .close, {_modalRoot.ToString()} [aria-label='Close']"));
            closeBtn.Click();
        }
    }
}