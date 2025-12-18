Feature: Make a Payment - No Late Fee Message

  Background:
    Given I launch the customer servicing application
    And I am on the Sign-In screen
    And I log in with valid customer credentials
    And I complete MFA verification
    And I am redirected to the dashboard
    And I dismiss any pop-ups
    And I select the applicable loan account

  Scenario Outline: Make a payment with payment date less than 15 days past due should not show late fee message
    When I click Make a Payment
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then no late fee message should be displayed

    Examples:
      | LoanNumber | PaymentDate           | ExpectedLateFee |
      | 3616       | 2025-12-20 00:00:00   | False           |