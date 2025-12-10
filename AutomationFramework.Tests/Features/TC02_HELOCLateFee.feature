Feature: TC02 - Verify no late fee message is displayed when payment date is exactly 15 days past due

  @TC02
  Scenario: TC02 - Verify no late fee message is displayed when payment date is exactly 15 days past due
    Given I launch the application
    And I login with encrypted credentials
    And I complete MFA
    And I am on the dashboard
    And I dismiss any popups
    And I select the applicable loan account
    When I navigate to Make a Payment
    And I handle any scheduled payment popup
    And I open the payment date picker
    And I select the payment date exactly 15 days past due
    Then I should not see the late fee message