using TechTalk.SpecFlow;
using AutomationFramework.Core.Base;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            DriverFactory.GetDriver();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DriverFactory.QuitDriver();
        }
    }
}