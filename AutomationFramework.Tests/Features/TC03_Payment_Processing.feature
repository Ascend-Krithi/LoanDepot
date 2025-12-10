# Automation for TestCaseID: TC03
@TC03
Feature: TC03 - Payment Processing
  As a user
  I want to process a payment
  So that my transaction is completed

  Scenario: Process payment
    Given I am on the Payment Page
    When I enter payment details
    And I submit the payment
    Then I should see a payment success message