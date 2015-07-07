using UnityEngine;
using System.Collections;

public class DebugMenu : MonoBehaviour {
	// Declare variables
	// Boolean to check whether debug is enabled or not
	bool debug = false;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SetUpDebugMenu() {

	}

	public void SwitchDebugState() {
		debug = !debug;
	}
}
