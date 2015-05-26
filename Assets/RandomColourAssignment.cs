using UnityEngine;
using System.Collections;

public class RandomColourAssignment : MonoBehaviour {
	// Declare variables
	// Set the number of passes to determine the complexity of the target colour
	[SerializeField] int numColours = 4;

	// Get a reference to the material being used by the target
	[SerializeField] Material colourCube;

	// Create an array to store the colours that have been randomly assigned
	private int[] colourArray;

	// Initialise all required values
	void Awake () {
		// Create a new instance of the colourArray
		colourArray = new int[3];

		// Set an initial colour for the target
		CreateColour();
	}

	public void CreateColour() {
		// Set all the starting colour values to 0
		for (int i = 0; i < colourArray.Length; i++) {
			colourArray[i] = 0;
		}
		
		// Set the current number of colour options
		if (PlayerPrefs.GetInt("numColours") == 0) {
			PlayerPrefs.SetInt("numColours", numColours);
		} else {
			numColours = PlayerPrefs.GetInt("numColours");
		}

		// Loop through the array the required number of times, and assign values to each colour
		for (int i = 0; i < numColours; i++) {
			colourArray[Random.Range(0,3)]++;
		}

		// Determine the greates value in the colour list
		int maxColour = colourArray[0];
		
		for (int i = 1; i < colourArray.Length; i++) {
			if (colourArray[i] > maxColour) {
				maxColour = colourArray[i];
			}
		}

		// Calculate each colour as a percentage of white
		float r = (255.0f / maxColour) * colourArray[0];
		float g = (255.0f / maxColour) * colourArray[1];
		float b = (255.0f / maxColour) * colourArray[2];

		// Assign the colour to the cube (Observing the / by 255 requirement)		
		colourCube.SetColor("_Color", new Color(r / 255.0f, g / 255.0f, b / 255.0f));
	}

	public int[] GetColourArray() {
		return colourArray;
	}
}
