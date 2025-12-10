@TC03
Feature: Verify late fee message is displayed when payment date is more than 15 days past due

  Scenario: Late fee message for payment date > 15 days past due
    Given the customer servicing application is launched
    And the user logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And all pop-ups are dismissed
    And the applicable loan account is selected
    And the user clicks Make a Payment
    And continues past any scheduled payment popup
    And opens the date picker
    And selects a payment date more than 15 days past due from test data
    When the user observes the late-fee message area
    Then a late fee message should be displayed