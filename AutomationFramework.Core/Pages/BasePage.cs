// AutomationFramework.Core/Pages/BasePage.cs

using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// Base class for all Page Objects.
    /// Provides shared functionalities like the driver and wait helpers.
    /// </summary>
    public abstract class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;
        protected readonly WaitHelper Wait;

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
            Wait = new WaitHelper(Driver);
        }
    }
}