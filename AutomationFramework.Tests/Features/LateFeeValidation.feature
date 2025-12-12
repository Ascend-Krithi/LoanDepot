Feature: HELOC Late Fee Validation

  As a loanDepot customer
  I want to verify late fee message behavior based on payment date

  Scenario Outline: Validate late fee message for payment date
    Given I launch the customer servicing application
    And I log in using valid credentials
    And I complete MFA verification
    And I am on the dashboard page
    And I close any pop-ups if present
    And I select the loan account "<LoanNumber>"
    And I click Make a Payment
    And I continue past any scheduled payment popup
    And I open the payment date picker
    When I select the payment date "<PaymentDate>"
    Then the late fee message area should display "<ExpectedLateFee>"

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-10   | TX    | False           |
      | TC02       | 3616       | 2025-12-16   | TX    | False           |
      | TC03       | 2805       | 2025-12-17   | TX    | True            |