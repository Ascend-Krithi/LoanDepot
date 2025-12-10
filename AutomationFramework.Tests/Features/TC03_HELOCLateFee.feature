@TC03
Feature: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due

  Background:
    Given the user has valid credentials
    And loan data, due date, and payment date (>15 days) provided externally

  Scenario: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due
    Given the customer servicing application is launched
    When the user logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And closes or dismisses any pop-ups
    And selects the applicable loan account
    And clicks Make a Payment
    And if scheduled payment popup appears, clicks Continue
    And opens the date picker
    And selects the payment date from test data (more than 15 days late)
    Then late fee message appears