Feature: Login and Users Management

  As a user of the application
  I want to login and navigate to the Users section
  So that I can verify access and page content

  @TC01
  Scenario: Login with valid credentials
    Given I am on the Login Page
    When I login with valid credentials
    Then I should be redirected to the Dashboard

  @TC02
  Scenario: Login with invalid credentials
    Given I am on the Login Page
    When I login with invalid credentials
    Then I should see an error message

  @TC03
  Scenario: Navigate to the Users section and verify the page title
    Given I am logged in with valid credentials
    When I navigate to the Users section
    Then the Users page title should be correct