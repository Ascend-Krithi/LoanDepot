@TC03
Feature: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due

  Background:
    Given the user has valid credentials and a HELOC loan available

  Scenario: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due
    When the user launches the customer servicing application
    And logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And closes or dismisses any pop-ups if they appear
    And selects the applicable loan account
    And clicks Make a Payment
    And if a scheduled payment popup appears, clicks Continue
    And opens the date picker
    And selects the payment date from test data more than 15 days past due
    Then a late fee message appears