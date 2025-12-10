# Automation for TestCaseID: TC01
@TC01
Feature: TC01 - Login Success
  As a valid user
  I want to login successfully
  So that I can access the dashboard

  Scenario: Successful login
    Given I am on the Login Page
    When I enter valid credentials
    And I click the Login button
    Then I should see the Dashboard