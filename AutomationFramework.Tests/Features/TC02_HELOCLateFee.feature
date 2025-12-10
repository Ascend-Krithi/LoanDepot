@TC02
Feature: TC02 - Verify no late fee message is displayed when payment date is exactly 15 days past due

  Background:
    Given the user has valid credentials
    And loan details and payment date are provided in test data

  Scenario: TC02 - Verify no late fee message is displayed when payment date is exactly 15 days past due
    Given the application is launched
    When the user logs in using valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And closes or dismisses any pop-ups
    And selects the applicable loan account
    And clicks Make a Payment
    And if scheduled payment popup appears, clicks Continue
    And opens the payment date picker
    And selects the payment date from test data (exactly 15 days past due)
    Then no late fee message should be displayed