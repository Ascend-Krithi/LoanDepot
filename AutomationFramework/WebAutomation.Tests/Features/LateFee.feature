Feature: Late Fee Message Validation

  As a customer servicing user
  I want to verify that no late fee message is displayed when payment date is less than 15 days past due
  So that customers are not incorrectly warned about late fees

  Scenario Outline: No late fee message is displayed for payment date < 15 days past due
    Given the customer servicing application is launched
    And the user logs in with valid credentials
    And the user completes MFA verification
    And the dashboard is loaded
    And all pop-ups are dismissed
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And the user continues past any scheduled payment popup
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then no late fee message is displayed

    Examples:
      | TestCaseId                | LoanNumber | PaymentDate  | ExpectedLateFee |
      | HAP-700 TS-001 TC-001     | 3616       | 2025-12-20   | False           |