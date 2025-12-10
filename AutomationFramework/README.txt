AutomationFramework - LD_Servicing_Automation

Overview:
This solution provides a compile-ready C# Selenium NUnit automation framework following the Page Object Model (POM) pattern for LD_Servicing_Automation functionalities. Locator mapping and Page Object generation that depend on a Locator JSON are omitted as requested. The framework uses the available Test Cases (TC01, TC02, TC03), Screen Flow, and references TestData.xlsx.

Key Points:
- POM adherence with page class stubs and method signatures.
- No element locators included due to missing Locator JSON.
- Compile-ready with NUnit and Selenium package references.
- Data provider stub references TestData.xlsx sheets/columns per the test cases.
- Screen Flow.txt is parsed to provide navigation order.

How to run:
1) Restore NuGet packages.
2) Ensure a Chrome WebDriver is available on PATH or in the bin folder (matching Chrome version).
3) Update appsettings.json:
   - BaseUrl to your LD_Servicing site.
   - TestDataPath to the actual location of TestData.xlsx.
   - ScreenFlowPath to the actual location of Screen Flow.txt.
4) Execute tests via dotnet test or your IDE's test runner.

Note:
- Without locator mappings, page methods log intended actions and return placeholder results enabling compile and basic logical flow.
- Replace method bodies with actual Selenium interactions once Locator JSON and locators become available.