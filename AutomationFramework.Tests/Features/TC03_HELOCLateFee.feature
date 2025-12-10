Feature: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due

  @TC03
  Scenario: TC03 - Verify late fee message is displayed when payment date is more than 15 days past due
    Given I launch the application
    And I login with encrypted credentials
    And I complete MFA
    And I am on the dashboard
    And I dismiss any popups
    And I select the applicable loan account
    When I navigate to Make a Payment
    And I handle any scheduled payment popup
    And I open the payment date picker
    And I select the payment date more than 15 days past due
    Then I should see the late fee message