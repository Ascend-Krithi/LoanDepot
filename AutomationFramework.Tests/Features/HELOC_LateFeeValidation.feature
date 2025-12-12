Feature: HELOC Late Fee Validation
  As a HELOC customer
  I want to ensure that no late fee message is displayed when payment date is less than 15 days past due

  Background:
    Given I launch the customer servicing application
    And I log in with valid customer credentials
    And I complete MFA verification
    And I am on the dashboard page
    And I dismiss any pop-ups

  Scenario: Verify no late fee message is displayed when payment date is less than 15 days past due
    Given I select the applicable loan account from test data
    When I click the Make a Payment button
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date from test data
    Then I should not see a late fee message