Feature: Make Payment
  As a user
  I want to make a payment on my loan account
  So that my payment is processed correctly

  @MakePayment
  Scenario Outline: Make a payment for a loan account (TestCaseId: <TestCaseId>)
    Given I am on the Login page
    When I login with valid credentials
    And I complete MFA authentication
    And I enter the OTP code
    And I select the loan account "<LoanAccount>"
    And I navigate to the Make Payment page
    And I schedule a payment for "<PaymentDate>" with amount "<Amount>" and memo "<Memo>"
    Then the payment should be processed successfully
    And the late fee message displayed should be "<ExpectedLateFee>"

    Examples:
      | TestCaseId | LoanAccount | PaymentDate | Amount | Memo | ExpectedLateFee |
      | TC001      | <LoanAccount> | <PaymentDate> | <Amount> | <Memo> | <ExpectedLateFee> |