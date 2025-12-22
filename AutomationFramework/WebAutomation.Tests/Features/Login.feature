Feature: Login

    As a user,
    I want to log in to the application,
    So that I can access my account.

@smoke
Scenario: Successful Login
    Given I am on the login page
    When I enter valid credentials
    And I click the login button
    Then I should be redirected to the dashboard