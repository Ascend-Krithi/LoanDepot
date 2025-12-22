Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message is not displayed for payment date less than 15 days past due
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account "<LoanNumber>"
    When the user clicks Make a Payment
    And continues past the scheduled payment popup if it appears
    And opens the payment date picker
    And selects the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  |
      | TC01       | 3616       | 2025-12-20   |