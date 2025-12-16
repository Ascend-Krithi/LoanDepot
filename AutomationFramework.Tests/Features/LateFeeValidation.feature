Feature: Late Fee Validation
  As a customer
  I want to verify late fee message behavior based on payment date

  Scenario Outline: Validate late fee message for HELOC payment
    Given I launch the customer servicing application
    And I log in with valid credentials
    And I complete MFA verification
    And I navigate to the dashboard
    And I close any pop-ups if present
    And I select the loan account "<LoanNumber>"
    And I click Make a Payment
    And I handle scheduled payment popup if present
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then the late fee message should be <ExpectedLateFee>

    Examples:
      | TestCaseId | LoanNumber | PaymentDate | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-10  | TX    | false           |
      | TC02       | 3616       | 2025-12-16  | TX    | false           |
      | TC03       | 2805       | 2025-12-17  | TX    | true            |