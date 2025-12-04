Feature: Login
  As a user
  I want to login to the application
  So that I can access my dashboard

  Scenario: Successful login
    Given I am on the login page
    When I enter valid credentials
    And I click the login button
    Then I should see the dashboard welcome message