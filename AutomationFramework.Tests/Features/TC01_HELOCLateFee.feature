@TC01
Feature: TC01 - Verify no late fee message is displayed when payment date is less than 15 days past due

  Background:
    Given the user has valid credentials
    And the applicable HELOC loan is available

  Scenario: TC01 - Verify no late fee message is displayed when payment date is less than 15 days past due
    Given the application is launched
    When the user logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And closes or dismisses any pop-ups if they appear
    And selects the applicable loan account
    And clicks Make a Payment
    And if a scheduled payment popup appears, clicks Continue
    And opens the payment date picker
    And selects the payment date from test data (less than 15 days past due)
    Then no late fee message is displayed