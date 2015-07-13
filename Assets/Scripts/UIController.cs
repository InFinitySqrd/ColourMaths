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

	// Get a reference to the label used to display the number of remaining colours
	[SerializeField] Text coloursRemaining;

	// Bool to determine if the user is releasing their click
	bool mouseDown = false;

	// The button that the user pressed
	Collider buttonPressed;

	// Function called every frame
	void Update() {
#if UNITY_EDITOR
		// Handle enabling/disabling the debug menu
		if (Input.GetKeyDown(KeyCode.Space)) {
			debugControls.SwitchDebugState();
		}
		
		if (Input.GetMouseButtonDown(0)) {
			mouseDown = true;
			HandleColourInput();
		}
		
		if (Input.GetMouseButtonUp(0)) {
			mouseDown = false;
			HandleColourInput();
		}
#elif UNITY_IOS || UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
		// Handle enabling/disabling the debug menu
		if (Input.touches.Length > 2 && Input.touches[2].phase == TouchPhase.Began) {
			debugControls.SwitchDebugState();
		}
		
		if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began) {
			mouseDown = true;
			HandleColourInput();
		}
		
		if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended) {
			mouseDown = false;
			HandleColourInput();
		}
#endif


		// Update the player's score value
		scoreLabel.text = PlayerPrefs.GetInt("score").ToString();
		coloursRemaining.text = "Colours: " + (goalColour.GetNumColours() - playerColour.GetNumColoursEntered()).ToString();
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

		if (mouseDown && Physics.Raycast(ray, out hit)) {
			// Set the button that was clicked
			buttonPressed = hit.collider;

			// Start the clicked animation
			hit.collider.GetComponent<ButtonClickedAnimation>().StartClickedAnimation();

		} else if (Physics.Raycast(ray, out hit)){
			if (hit.collider == buttonPressed) {
				switch (buttonPressed.name) {
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
	}

	private void IncrementPlayerColour(int index) {
		if (playerColour.GetNumColoursEntered() < goalColour.GetNumColours()) {
			// Increment the current colour
			playerColour.IncrementCurrentColour(index);

			// Increment the number of colours added by the player
			playerColour.SetNumColours(playerColour.GetNumColoursEntered() + 1);
			
			// Assign the new colour to the player
			playerColour.UpdatePlayerColour();
		}
	}

	IEnumerator SendAnalyticsEvent(string eventString) {
		GameAnalytics.NewDesignEvent(eventString);
		yield return null;
	}
}
