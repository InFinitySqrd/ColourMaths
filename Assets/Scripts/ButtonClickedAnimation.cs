using UnityEngine;
using System.Collections;

public class ButtonClickedAnimation : MonoBehaviour {
	// Declare variables
	// Variable to control the speed at which the object will scale up
	[SerializeField] float animationSpeed = 1.0f;

	// Variable to control the size the button will be when it is pressed
	[SerializeField] Vector3 targetSize;

	// Variable to store the transform of the button
	private Transform buttonObject;

	// Variable to store the original size of the button
	private Vector3 originalSize;

	// Variable to check whether this was the object that was clicked or not
	private bool clicked = false;

	// Use for initialisation
	void Awake() {
		buttonObject = this.transform;
		originalSize = buttonObject.localScale;
	}

	// Method that runs each frame
	void Update() {
		// If the user releases the button, scale back up to full size
		if (clicked && Input.GetMouseButtonUp(0)) {
			StopCoroutine("ButtonPressed");
			StartCoroutine("ButtonReleased");
		}
	}

	// Public method that is called to start the click process
	public void StartClickedAnimation() {
		clicked = true;
		StartCoroutine("ButtonPressed");
	}

	IEnumerator ButtonPressed() {
		while (buttonObject.localScale.x > targetSize.x) {
			// Get a reference to the starting scale
			Vector3 newScale;
			newScale = buttonObject.localScale;

			// Scale the object
			newScale = Vector3.MoveTowards(buttonObject.localScale, targetSize, Time.deltaTime * animationSpeed);

			// Assign the new scale
			buttonObject.localScale = newScale;

			yield return null;
		}
	}

	IEnumerator ButtonReleased() {
		while (buttonObject.localScale.x < originalSize.x) {
			// Get a reference to the starting scale
			Vector3 newScale;
			newScale = buttonObject.localScale;
			
			// Scale the object
			newScale = Vector3.MoveTowards(buttonObject.localScale, originalSize, Time.deltaTime * animationSpeed);
			
			// Assign the new scale
			buttonObject.localScale = newScale;
			yield return null;
		}
	}
}
