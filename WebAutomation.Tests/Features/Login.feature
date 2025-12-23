Feature: Login

  Scenario Outline: User logs in with valid credentials
    Given the user is on the login page
    When the user logs in with TestCaseId "<TestCaseId>"
    Then the dashboard is displayed

    Examples:
      | TestCaseId |
      | TC001      |