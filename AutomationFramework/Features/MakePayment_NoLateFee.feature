Feature: Make a Payment Without Late Fee

  Scenario: HAP-700 TS-001 TC-001 - Make a payment for a HELOC loan with no late fee
    Given the customer servicing application is launched
    And I log in with valid customer credentials
    And I complete MFA verification
    And I am on the dashboard
    And I dismiss any pop-ups if they appear
    When I select the applicable loan account
    And I click Make a Payment
    And I continue past any scheduled payment popup
    And I open the payment date picker
    And I select the payment date from test data
    Then no late fee message is displayed