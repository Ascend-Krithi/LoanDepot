import os
import requests
import json

# --- Configuration ---
# It's best practice to use environment variables for sensitive data.
TESTRAIL_URL = os.environ.get("TESTRAIL_URL", "https://your-instance.testrail.io/")
TESTRAIL_USER = os.environ.get("TESTRAIL_USER", "user@example.com")
TESTRAIL_API_KEY = os.environ.get("TESTRAIL_API_KEY", "your_api_key")

# Parameters from the request
PROJECT_ID = 3
SUITE_ID = 8
SECTION_ID = 32

def format_test_case(case):
    """
    Formats a single TestRail case into the desired JSON structure.
    Handles missing fields gracefully using .get().
    """
    # The 'custom_steps_separated' field usually contains the steps.
    # If not present, default to an empty list.
    raw_steps = case.get('custom_steps_separated', [])
    formatted_steps = []
    if raw_steps: # Ensure raw_steps is not None and not empty
        for index, step in enumerate(raw_steps):
            formatted_steps.append({
                "Step_ID": index + 1, # Use index as Step_ID as it's not a standard field
                "Step_Desc": step.get('content'),
                "Expected_Result": step.get('expected'),
                # Assuming 'Test_Data' is a custom field within the step.
                "Test_Data": step.get('custom_test_data', None)
            })

    return {
        "testCaseId": case.get('id'),
        "testCaseDescription": case.get('title'),
        # Mapping assumed custom fields. Use .get() for safety.
        "AcceptanceCriteriaId": case.get('custom_acceptance_criteria_id', None),
        "IssueId": case.get('refs', None),
        "testScenarioId": case.get('custom_test_scenario_id', None),
        "testSteps": formatted_steps
    }

def fetch_test_cases(project_id, suite_id, section_id):
    """
    Fetches all test cases from TestRail for a given project, suite, and section.
    """
    api_url = f"{TESTRAIL_URL}index.php?/api/v2/get_cases/{project_id}&suite_id={suite_id}&section_id={section_id}"
    
    all_cases = []
    
    try:
        response = requests.get(
            api_url,
            auth=(TESTRAIL_USER, TESTRAIL_API_KEY),
            headers={'Content-Type': 'application/json'}
        )
        
        # Raise an exception for bad status codes (4xx or 5xx)
        response.raise_for_status()
        
        # The primary response is a JSON object with a 'cases' key
        data = response.json()
        raw_cases = data.get('cases', [])

        if not raw_cases:
            print(f"Warning: No test cases found for Project ID: {project_id}, Suite ID: {suite_id}, Section ID: {section_id}")
            return []

        for case in raw_cases:
            all_cases.append(format_test_case(case))
            
        return all_cases

    except requests.exceptions.RequestException as e:
        print(f"Error fetching data from TestRail: {e}")
        return None
    except json.JSONDecodeError as e:
        print(f"Error decoding JSON from TestRail response: {e}")
        return None

if __name__ == "__main__":
    print(f"Fetching test cases for Project ID: {PROJECT_ID}, Suite ID: {SUITE_ID}, Section ID: {SECTION_ID}")
    
    test_cases_data = fetch_test_cases(PROJECT_ID, SUITE_ID, SECTION_ID)
    
    if test_cases_data is not None:
        # Output the final result as a single JSON array string
        final_json_output = json.dumps(test_cases_data, indent=4)
        print("\n--- Formatted Test Cases ---")
        print(final_json_output)
        
        # Optionally, save to a file
        with open("testrail_cases.json", "w") as f:
            f.write(final_json_output)
        print("\nData saved to testrail_cases.json")
