Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message for payment date
    Given the customer servicing application is launched
    And I log in as a valid customer
    And I complete MFA verification
    And I am on the dashboard
    And I dismiss any pop-ups if present
    And I select the applicable loan account
    And I click Make a Payment
    And I handle scheduled payment popup if present
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId                                 | LoanNumber | PaymentDate | State |
      | Test Case HAP-700 TS-001 TC-001            | 3616       | 2025-12-20  | TX   |