using OpenQA.Selenium;

namespace WebAutomation.Core.Utilities
{
    /// <summary>
    /// Framework-level handler to suppress ServisBot chat iframe.
    /// This must NEVER be implemented in test code.
    /// </summary>
    public sealed class ChatBotHandler
    {
        private readonly IWebDriver _driver;

        public ChatBotHandler(IWebDriver driver)
        {
            _driver = driver;
        }

        public void HideIfPresent()
        {
            try
            {
                var js = (IJavaScriptExecutor)_driver;

                js.ExecuteScript(@"
                    document
                        .querySelectorAll(""iframe[id*='servisbot'], iframe[class*='sb-iframe']"")
                        .forEach(f => {
                            f.style.display = 'none';
                            f.style.visibility = 'hidden';
                        });
                ");
            }
            catch
            {
                // Intentionally ignored – chatbot may not exist
            }
        }
    }
}