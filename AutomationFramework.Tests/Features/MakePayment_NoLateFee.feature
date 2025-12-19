Feature: Make a Payment with No Late Fee

  Background:
    Given I launch the customer servicing application
    And I am on the Sign-In screen
    And I log in with valid customer credentials
    And I complete MFA verification
    And I am redirected to the dashboard
    And I dismiss any pop-ups

  Scenario: HAP-700 TS-001 TC-001 - Make a payment for a HELOC loan with no late fee
    When I select the applicable loan account from test data
    And I click Make a Payment
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date from test data
    Then no late fee message should be displayed