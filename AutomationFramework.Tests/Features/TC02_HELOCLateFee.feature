@TC02
Feature: TC02 - Verify no late fee message is displayed when payment date is exactly 15 days past due

  Background:
    Given the user has valid credentials
    And loan details and payment date are provided in test data

  Scenario: TC02 - No late fee message for payment exactly 15 days past due
    Given the user launches the application
    When the user logs in using valid credentials
    And the user completes MFA verification
    And the user navigates to the dashboard
    And the user closes/dismisses any pop-ups
    And the user selects the applicable loan account
    And the user clicks Make a Payment
    And if scheduled payment popup appears, the user clicks Continue
    And the user opens the payment date picker
    And the user selects the payment date from test data (exactly 15 days past due)
    Then no late fee message should be displayed