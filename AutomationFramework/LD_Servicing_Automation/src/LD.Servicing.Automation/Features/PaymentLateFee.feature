Feature: Payment Late Fee Validation
  As a servicing application user
  I want to see late fee messaging based on selected payment date
  So that I can understand additional fees when paying late

  Background:
    Given I launch the LD Servicing application
    And I log in with valid credentials
    And I complete MFA
    And I am on the dashboard and dismiss any popups
    And I select a loan account and navigate to payment

  @TC01
  Scenario: Verify no late fee message is displayed when payment date is less than 15 days past due
    When I select a payment date that is less than 15 days past due
    Then I should not see a late fee message

  @TC02
  Scenario: Verify no late fee message is displayed when payment date is exactly 15 days past due
    When I select a payment date that is exactly 15 days past due
    Then I should not see a late fee message

  @TC03
  Scenario: Verify late fee message is displayed when payment date is more than 15 days past due
    When I select a payment date that is more than 15 days past due
    Then I should see a late fee message