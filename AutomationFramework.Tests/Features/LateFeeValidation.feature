Feature: Late Fee Message Validation
  As a HELOC customer
  I want to ensure the late fee message is displayed only when appropriate
  So that I am accurately informed about late fees

  @regression
  Scenario Outline: Validate late fee message for payment date
    Given I launch the application
    And I log in with valid credentials
    And I complete MFA verification
    And I am on the dashboard
    And I dismiss any popups
    And I select the loan account "<LoanNumber>"
    When I click Make a Payment
    And I continue past any scheduled payment popup
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then the late fee message displayed should be <ExpectedLateFee>
    Examples:
      | TestCaseId | LoanNumber | PaymentDate | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20  | false           |
      | TC02       | 3616       | 2025-12-31  | false           |
      | TC03       | 2805       | 2026-01-17  | true            |