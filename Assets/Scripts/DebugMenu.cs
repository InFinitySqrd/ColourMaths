using UnityEngine;
using System.Collections;
using System.IO;

public class DebugMenu : MonoBehaviour {
	// Declare variables
	// Boolean to check whether debug is enabled or not
	private bool debug = false;

	// Get a reference to the target colour, so it can be reset at any time
	[SerializeField] RandomColourAssignment goalColour;

	// Get a reference to the animation script, so that variables can be edited
	[SerializeField] SuccessAnimation animationVars;

	// Variable to hold the current difficulty temporarily
	private int currentDifficulty = 0;

	// Use this for initialization
	void Awake () {

	}
	
	void OnGUI() {
		DrawDebugMenu();
	}

	private void DrawDebugMenu() {
		if (debug) {
			// Slider to change the game difficulty
			DrawSliders(0, "Difficulty", ref currentDifficulty, 1, 20);

			// Sliders to control the particle effects
			DrawSliders(1, "Num Particles", ref animationVars.numParticles, 0, 50);
			DrawSliders(2, "Particle Speed", ref animationVars.particleSpeed, 0.0f, 10.0f);
			DrawSliders(3, "Particle Dist", ref animationVars.particleMaxDist, 0.0f, 10.0f);

			// Sliders to control the animation
			DrawSliders(4, "Stretch Speed", ref animationVars.stretchSpeed, 0.0f, 20.0f);
			DrawSliders(5, "Snap Speed", ref animationVars.snapSpeed, 0.0f, 20.0f);
			DrawSliders(6, "Stretch Decay", ref animationVars.stretchDecay, 0.0f, 0.085f);

			// Draw a button to output all variables to a txt file
			SaveVariables();
		}
	}

	// Sliders to work with floating point numbers
	private void DrawSliders(int lineNum, string labelName, ref float editedVar, float sliderMinVal, float sliderMaxVal) {
		// Draw a slider on the screen to make a variable editable on device
		GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), labelName);
		
		float sliderValue = editedVar;
		editedVar = GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f), sliderValue, sliderMinVal, sliderMaxVal);
		
		GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
	}

	// Sliders to work with integer numbers
	private void DrawSliders(int lineNum, string labelName, ref int editedVar, int sliderMinVal, int sliderMaxVal) {
		// Draw a slider on the screen to make a variable editable on device
		GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), labelName);
		
		int sliderValue = editedVar;
		editedVar = (int)GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f), sliderValue, sliderMinVal, sliderMaxVal);
		
		GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
	}

	private void SaveVariables() {
		// Button that creates a text file and saves all current variables to it
		if (GUI.Button(new Rect(Screen.width / 2.0f - Screen.width / 10.0f, 0.0f, Screen.width / 5.0f, Screen.width / 5.0f), "Save Vars")) {
			if (!Directory.Exists(Application.persistentDataPath)) {
				Directory.CreateDirectory(Application.persistentDataPath);
			}

			string outputText = animationVars.numParticles + "," + animationVars.particleSpeed + "," + animationVars.particleMaxDist + "," +
								animationVars.stretchSpeed + "," + animationVars.snapSpeed + "," + animationVars.stretchDecay;
			string uniqueName = System.DateTime.Now.TimeOfDay.ToString();
			uniqueName = uniqueName.Replace(':', '-');

			File.WriteAllText(Application.persistentDataPath + "/" + uniqueName + "VARS.txt", outputText);
		}
	}

	// Switch the state of the debug menu, and handle everything accordingly
	public void SwitchDebugState() {
		debug = !debug;

		// Get the current difficulty level
		if (debug) {
			currentDifficulty = PlayerPrefs.GetInt("numColours");
		}

		// Set the game's difficulty to the new value
		if (!debug) {
			// Update the game difficulty
			PlayerPrefs.SetInt("numColours", (int)currentDifficulty);
			
			// Reset the score upon changing the difficulty
			PlayerPrefs.SetInt("score", 0);
			
			// Create a new colour puzzle
			goalColour.CreateColour();
		}
	}
}
