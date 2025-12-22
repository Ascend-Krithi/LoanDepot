Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message for payment date less than 15 days past due
    Given the user launches the customer servicing application
    And logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account "<LoanNumber>"
    When the user clicks Make a Payment
    And continues past any scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId                              | LoanNumber | PaymentDate  |
      | Test Case HAP-700 TS-001 TC-001         | 3616       | 2025-12-20   |