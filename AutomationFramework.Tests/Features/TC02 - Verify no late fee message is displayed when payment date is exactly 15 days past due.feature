@TC02
Feature: Verify no late fee message is displayed when payment date is exactly 15 days past due

  Scenario: No late fee message for payment date = 15 days past due
    Given the application is launched
    And the user logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And all pop-ups are dismissed
    And the applicable loan account is selected
    And the user clicks Make a Payment
    And continues past any scheduled payment popup
    And opens the payment date picker
    And selects a payment date exactly 15 days past due from test data
    When the user observes the late-fee message area
    Then no late fee message should be displayed