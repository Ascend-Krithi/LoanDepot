Feature: Login

  Scenario: Successful login and loan selection
    Given I login with valid credentials
    When I select "Home Loan" from the loan dropdown
    And I select "My Home Loan" from the loan list
    And I dismiss any popup
    And I handle delayed chat popup
    And I select "2024-07-01" in the date picker
    Then I should see the message "Loan selected successfully"