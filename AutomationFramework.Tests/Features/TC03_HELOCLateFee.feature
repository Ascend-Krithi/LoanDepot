@TC03
Feature: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due

  Background:
    Given the user has valid credentials
    And loan data, due date, and payment date (>15 days) provided externally

  Scenario: TC03 - Late fee message for payment >15 days past due
    Given the user launches the customer servicing application
    When the user logs in using valid customer credentials
    And the user completes MFA verification
    And the user navigates to the dashboard
    And the user closes/dismisses any pop-ups
    And the user selects the applicable loan account
    And the user clicks Make a Payment
    And if scheduled payment popup appears, the user clicks Continue
    And the user opens the date picker
    And the user selects the payment date from test data (more than 15 days late)
    Then a late fee message is displayed