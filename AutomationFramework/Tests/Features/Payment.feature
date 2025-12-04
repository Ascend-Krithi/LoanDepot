Feature: Payment

  Scenario: Make a loan payment
    Given I am logged in as "testuser"
    And I select loan "Personal Loan"
    And I dismiss chat popup if present
    When I select payment type "EMI"
    And I select payment date "2024-07-01"
    And I submit the payment
    Then I should see message "Payment successful"