using UnityEngine;
using System.Collections;

public class Imposter : MonoBehaviour {

	[SerializeField] static protected Story story;
	[SerializeField] int activeIteration;
	[SerializeField] float lightFlashingFrequency;
	[SerializeField] float callOutSpacing;

	AudioSource player;
	bool active = false;
	
	float imposterLightMaxIntensity;
	float lastCallOutTime;
	Light imposterLight;


	// Use this for initialization
	void Start () {
		if (Imposter.story == null)
			Imposter.story = GameObject.FindObjectOfType<Story>();

		active = Imposter.story.PlayIteration == activeIteration;
		player = GetComponent<AudioSource>();
		imposterLight = GetComponent<Light>();
		imposterLightMaxIntensity = imposterLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (IsCallOutTime)
				CallOut();

			FlashLight();
		} else
			imposterLight.enabled = false;
	}

	void CallOut() {
		player.PlayOneShot(story.ImposterVocalisation);
	}

	bool IsCallOutTime {
		get {
			return !player.isPlaying && Time.timeSinceLevelLoad - lastCallOutTime > callOutSpacing;
		}
	}

	void FlashLight() {
		imposterLight.intensity = Mathf.Pow(Mathf.Sin(Time.timeSinceLevelLoad * lightFlashingFrequency), 2) * imposterLightMaxIntensity; 
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player")
			Debug.Log("Found Active Imposter");
	}

}
