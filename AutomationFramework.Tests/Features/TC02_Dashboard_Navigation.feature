# Automation for TestCaseID: TC02
@TC02
Feature: TC02 - Dashboard Navigation
  As a logged-in user
  I want to navigate to the dashboard
  So that I can view my account summary

  Scenario: Navigate to dashboard
    Given I am logged in
    When I navigate to the Dashboard
    Then the Dashboard should be visible