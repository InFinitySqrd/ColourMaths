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

	// Variable to control how quickly the particles move
	[SerializeField] float particleSpeed = 1.0f;

	// Variable to control how far particles will move from the centre before being destroyed
	[SerializeField] float particleMaxDist = 5.0f;

	// Variables to control the number of particles that are spawned
	[SerializeField] int numParticles = 12;

	// Target size for the scale function
	[SerializeField] Vector3 targetStretchScale = new Vector3(1,1,1);
	[SerializeField] Vector3 targetSnapScale = new Vector3(1,1,1);

	// Speed variables to control the duration of the scale
	[SerializeField] float stretchSpeed = 1.0f;
	[SerializeField] float snapSpeed = 1.0f;

	// Decay variable to slow the speed of the stretch over time
	[SerializeField] float stretchDecay = 0.1f;

	// A variable to track the current scale rate
	private float scaleRate;

	// Variable to store all particles that have been spawned
	private List<GameObject> particles;

	void Awake () {
		particles = new List<GameObject>();
	}

	public void SetUpAnimation() {
		scaleRate = stretchSpeed;

		StartCoroutine("StretchAnimation");
	}

	private void SpawnParticles() {
		for (int i = 0; i < numParticles; i++) {
			// Spawn particle effect
			GameObject spawnedObject;
			spawnedObject = (GameObject)GameObject.Instantiate(particleEffect, this.transform.position, this.transform.rotation);
			spawnedObject.GetComponent<Renderer>().material.color = playerColour.GetTargetColour();
			particles.Add(spawnedObject);
		}

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

		StartCoroutine("SnapAnimation");
		SpawnParticles();
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
}
