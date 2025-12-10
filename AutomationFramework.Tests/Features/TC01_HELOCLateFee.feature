@TC01
Feature: TC01 - Verify no late fee message is displayed when payment date is less than 15 days past due

  Background:
    Given the user has valid credentials
    And the applicable HELOC loan is available
    And due date, state, and payment date are provided in external test data

  Scenario: TC01 - No late fee message for payment <15 days past due
    Given the user launches the customer servicing application
    When the user logs in using valid customer credentials
    And the user completes MFA verification
    And the user navigates to the dashboard
    And the user closes/dismisses any pop-ups if they appear
    And the user selects the applicable loan account
    And the user clicks Make a Payment
    And if a scheduled payment popup appears, the user clicks Continue
    And the user opens the payment date picker
    And the user selects the payment date from test data (less than 15 days past due)
    Then no late fee message is displayed