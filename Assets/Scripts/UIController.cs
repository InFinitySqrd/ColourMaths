using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class UIController : MonoBehaviour {
	// Declare variables
	// Get a refernece to the goal colour
	[SerializeField] RandomColourAssignment goalColour;

	// Get a refernece to the player colour
	[SerializeField] HandlePlayerColour playerColour;

	// Get a reference to the debug menu, so that it can be enabled
	[SerializeField] DebugMenu debugControls;

	// Get a reference to the score label
	[SerializeField] Text scoreLabel;

	// Function called every frame
	void Update() {
		// Handle enabling/disabling the debug menu
		if ((Input.touches.Length > 2 && Input.touches[2].phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) {
			debugControls.SwitchDebugState();
		}

		if (Input.GetMouseButtonDown(0)) {
			HandleColourInput();
		}

		// Update the player's score value
		scoreLabel.text = PlayerPrefs.GetInt("score").ToString();
	}

	public void NewGame() {
		// Send analyitics event recording the colour the player gave up on
		string analyticsData = "TargetColour: " + goalColour.GetTargetColour().ToString() + "PlayerColour: " + playerColour.GetPlayerColour().ToString();
		StartCoroutine(SendAnalyticsEvent(analyticsData));

		// Restart the game 
		goalColour.CreateColour();
		playerColour.ClearPlayerColour();
	}

	public void ClearColour() {
		playerColour.ClearPlayerColour();
	}

	private void HandleColourInput() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit)) {
			switch (hit.collider.name) {
			case "Red":
				IncrementPlayerColour(0);
				break;
			case "Green":
				IncrementPlayerColour(1);
				break;
			case "Blue":
				IncrementPlayerColour(2);
				break;
			}
		}
	}

	private void IncrementPlayerColour(int index) {
		if (playerColour.GetNumColours() < goalColour.GetNumColours()) {
			// Increment the current colour
			playerColour.IncrementCurrentColour(index);

			// Increment the number of colours added by the player
			playerColour.SetNumColours(playerColour.GetNumColours() + 1);
			
			// Assign the new colour to the player
			playerColour.UpdatePlayerColour();
		}
	}

	IEnumerator SendAnalyticsEvent(string eventString) {
		GameAnalytics.NewDesignEvent(eventString);
		yield return null;
	}
}
