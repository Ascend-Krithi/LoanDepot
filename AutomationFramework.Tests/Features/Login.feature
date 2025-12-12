# AutomationFramework.Tests/Features/Login.feature

@Login
Feature: Login Functionality
    As a user
    I want to log into the application
    So that I can access my account details

Scenario: TC01 - Login with Valid Credentials
    Given the user is on the login page
    When the user enters valid credentials
    And the user clicks the login button
    Then the user should be redirected to the dashboard page

Scenario: TC02 - Login with Invalid Credentials
    Given the user is on the login page
    When the user enters invalid credentials
    And the user clicks the login button
    Then an error message 'Invalid credentials' should be displayed