using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	// Declare variables
	// Get a refernece to the goal colour
	[SerializeField] RandomColourAssignment goalColour;

	// Get a refernece to the player colour
	[SerializeField] HandlePlayerColour playerColour;

	// Get a reference to the debug menu, so that it can be enabled
	[SerializeField] DebugMenu debugControls;

	// Function called every frame
	void Update() {
		if ((Input.touches.Length > 2 && Input.touches[2].phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space)) {
			debugControls.SwitchDebugState();
		}
	}

	void OnGUI() {
		// Button to reset the game and get a new colour
		if (GUI.Button(new Rect(0.0f, 0.0f, Screen.width / 5.0f, Screen.width / 5.0f), "New Game")) {
			goalColour.CreateColour();
			playerColour.ClearPlayerColour();
		}

		// Button to clear previously entered colours
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5.0f, 0.0f, Screen.width / 5.0f, Screen.width / 5.0f), "Clear")) {
			playerColour.ClearPlayerColour();
		}

		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 32;
		style.alignment = TextAnchor.MiddleCenter;

		// Label to display the player's current score
		GUI.Box(new Rect(Screen.width / 2.0f - Screen.height / 10.0f, 0.0f, Screen.height / 5.0f, Screen.height / 5.0f), PlayerPrefs.GetInt("score").ToString(), style);

		DrawColourButtons();
	}

	private void DrawColourButtons() {
		// Button to add red to the colour
		if (GUI.Button(new Rect(0.0f, 4.0f * Screen.height / 5.0f, Screen.width / 3.0f, Screen.width / 3.0f), "Red")) {
			// Increment the value for the red colour component
			IncrementPlayerColour(0);
		}
		
		// Button to add green to the colour
		if (GUI.Button(new Rect(Screen.width / 3.0f, 4.0f * Screen.height / 5.0f, Screen.width / 3.0f, Screen.width / 3.0f), "Green")) {
			// Increment the value for the green colour component
			IncrementPlayerColour(1);
		}
		
		// Button to add blue to the colour
		if (GUI.Button(new Rect(2.0f * Screen.width / 3.0f, 4.0f * Screen.height / 5.0f, Screen.width / 3.0f, Screen.width / 3.0f), "Blue")) {
			// Increment the value for the blue colour component
			IncrementPlayerColour(2);
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
}
