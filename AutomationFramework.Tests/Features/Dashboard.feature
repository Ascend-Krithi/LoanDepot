# AutomationFramework.Tests/Features/Dashboard.feature

@Navigation
Feature: Dashboard Navigation
    As a logged-in user
    I want to navigate through the application
    So that I can view different sections

@LoggedIn
Scenario: TC03 - Navigate to the 'My Info' page
    Given the user is on the dashboard page
    When the user clicks on the 'My Info' link in the navigation menu
    Then the user should be redirected to the 'My Info' page