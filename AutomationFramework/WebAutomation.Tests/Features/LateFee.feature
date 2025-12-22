Feature: Late Fee Message Validation for HELOC Payment

  Scenario Outline: Validate late fee message for HELOC payment date selection
    Given the user launches the customer servicing application
    And logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account "<LoanNumber>"
    And clicks Make a Payment
    And continues past scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  |
      | TC01       | 3616       | 2025-12-20   |
      | TC03       | 3616       | 2026-01-16   |