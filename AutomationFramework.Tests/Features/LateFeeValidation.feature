Feature: Late Fee Validation
  As a HELOC customer
  I want to ensure that no late fee message is displayed when payment date is less than 15 days past due

  @TestCaseId=TC01
  Scenario Outline: Verify no late fee message is displayed when payment date is less than 15 days past due
    Given I launch the customer servicing application
    When I log in with valid customer credentials
    And I complete MFA verification
    And I navigate to the dashboard
    And I dismiss any pop-ups if present
    And I select the loan account "<LoanNumber>"
    And I click Make a Payment
    And I continue past any scheduled payment popup if present
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then no late fee message should be displayed

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-10   | TX    | False           |