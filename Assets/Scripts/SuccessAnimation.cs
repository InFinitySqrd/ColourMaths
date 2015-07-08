using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuccessAnimation : MonoBehaviour {
	// Declare variables
	// Reference to the object to animate
	[SerializeField] Transform animateObject;

	// Reference to the colour so that it can be cleared once the animation is complete
	[SerializeField] HandlePlayerColour playerColour;
	
	// Reference to an object that can be spawned as part of the animation
	[SerializeField] GameObject particleEffect;
	[SerializeField] GameObject particleEffect2;

	// Variable to control how quickly the particles move
	public float particleSpeed = 1.0f;

	// Variable to control how far particles will move from the centre before being destroyed
	public float particleMaxDist = 5.0f;

	// Variables to control the number of particles that are spawned
	public int numParticles = 12;

	// Target size for the scale function
	[SerializeField] Vector3 targetStretchScale = new Vector3(1,1,1);
	[SerializeField] Vector3 targetSnapScale = new Vector3(1,1,1);

	// Speed variables to control the duration of the scale
	public float stretchSpeed = 1.0f;
	public float snapSpeed = 1.0f;

	// Decay variable to slow the speed of the stretch over time
	public float stretchDecay = 0.1f;

	// A variable to track the current scale rate
	private float scaleRate;

	// Variable to store all particles that have been spawned
	private List<GameObject> particles;

	void Awake () {
		particles = new List<GameObject>();
	}

	public void SetUpAnimation() {
		scaleRate = stretchSpeed;
		
		animateObject.transform.localScale = targetSnapScale;
		StartCoroutine("StretchAnimation");
	}

	private void SpawnParticles() {
		// Create variables to track the spawn point
		GameObject targetObject = new GameObject();
		Transform spawnTarget = targetObject.transform;
		spawnTarget.position = this.transform.position;
		spawnTarget.rotation = this.transform.rotation;

		// Determine the spawn rotation and position that the particle should be created at
		for (int i = 0; i < numParticles; i++) {
			spawnTarget.Rotate(Vector3.forward, 360.0f / numParticles);
			spawnTarget.position = new Vector3 (spawnTarget.position.x, spawnTarget.position.y, this.transform.position.z - 1.0f);

			// Spawn particle effect
			GameObject spawnedObject;

			if (i % 2 == 0) {
				spawnedObject = (GameObject)GameObject.Instantiate(particleEffect, spawnTarget.position, spawnTarget.rotation);
			} else {
				spawnedObject = (GameObject)GameObject.Instantiate(particleEffect2, spawnTarget.position, spawnTarget.rotation);
			}

			spawnedObject.GetComponent<Renderer>().material.color = playerColour.GetTargetColour();
			particles.Add(spawnedObject);
		}

		// Destroy the temporary game object
		Destroy (targetObject);

		StartCoroutine("ParticleMovement");
	}

	IEnumerator ParticleMovement() {
		while (Vector3.Distance(particles[0].transform.position, this.transform.position) < particleMaxDist) {
			// Move each particle in the list
			foreach (GameObject particle in particles) {
				particle.transform.position += particle.transform.up * Time.deltaTime * particleSpeed;
			}

			yield return null;
		}

		// When all particles have been moved, destroy each of them
		foreach (GameObject particle in particles) {
			Destroy(particle.gameObject);
		}

		// Clear the list of particles, so that it is ready for the next effect
		particles = new List<GameObject>();
	}

	IEnumerator StretchAnimation() {
		while (animateObject.localScale.x < targetStretchScale.x) {
			// Scale up the object at a slowly decreasing rate
			Vector3 newScale;
			newScale = animateObject.localScale;			

			// Decriment the rate at which the object will scale
			scaleRate -= scaleRate * stretchDecay;
			newScale = Vector3.MoveTowards(animateObject.localScale, targetStretchScale, scaleRate * Time.deltaTime);
			
			animateObject.localScale = newScale;
			
			yield return null;
		}
		
		SwitchAnimatingStates();
	}

	IEnumerator SnapAnimation() {
		while (animateObject.localScale.x > targetSnapScale.x) {
			// Scale up the object at a slowly decreasing rate
			Vector3 newScale;
			newScale = animateObject.localScale;

			// Decriment the rate at which the object will scale
			newScale = Vector3.MoveTowards(animateObject.localScale, targetSnapScale, snapSpeed * Time.deltaTime);
			
			animateObject.localScale = newScale;
			
			yield return null;
		}

		playerColour.ClearPlayerColour();
	}

	private void SwitchAnimatingStates() {
		StartCoroutine("SnapAnimation");
		SpawnParticles();
	}

	public float DebugGetStretchTarget() {
		return targetStretchScale.x;
	}

	public void DebugSetStretchTarget(float targetSize) {
		targetStretchScale = new Vector3(targetSize, targetSize, targetSize);
	}
}
