Feature: Loan Application and Servicing Workflows
  As a user of the Servicing application
  I want to login, navigate, search loans, make payments, and view escrow details
  So that I can validate core flows

  Background:
    Given I am on the dashboard page

  @TR-1012
  Scenario: Login with valid credentials
    When I login with username "valid.user" and password "valid.password"
    Then I should see the dashboard home

  @TR-1013
  Scenario: Navigate to Loans from Dashboard
    When I login with username "valid.user" and password "valid.password"
    And I navigate to Loans
    Then I should see the loan grid

  @TR-1014
  Scenario Outline: Global search by Loan Number
    When I login with username "valid.user" and password "valid.password"
    And I search for loan "<LoanNumber>"
    And I open the first loan result
    Then I should see the loan details page

    Examples:
      | LoanNumber |
      | 1234567890 |

  @TR-1015
  Scenario: Make a Payment from Loan Details
    When I login with username "valid.user" and password "valid.password"
    And I search for loan "1234567890"
    And I open the first loan result
    And I make a payment with data row "1"
    Then I should see a payment success confirmation

  @TR-1016
  Scenario: View Escrow details tab
    When I login with username "valid.user" and password "valid.password"
    And I search for loan "1234567890"
    And I open the first loan result
    And I open the Escrow tab
    Then I should see the escrow details